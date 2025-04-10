using System.Collections.Generic;
using GameSaves;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RoundManager))]
public class LevelManager : MonoBehaviour
{
    protected static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
            {
                LevelManager[] instances = FindObjectsByType<LevelManager>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
                foreach (LevelManager instance in instances)
                {
                    if (instance._validInstance)
                    {
                        _instance = instance;
                        break;
                    }
                }
            }
            return _instance;
        }
    }

    public void RestartLevel()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [SerializeField] protected bool _encryptData = true;
    protected string _saveFileName = null;
    protected string saveFileName
    {
        get
        {
            if (string.IsNullOrEmpty(_saveFileName))
            {
                _saveFileName = SaveDataUtility._defaultSaveFile;
            }
            return _saveFileName;
        }
    }
    protected SaveData _saveData = null;
    public SaveData saveData
    {
        get
        {
            return _saveData;
        }
    }
    protected Shop _shop = null;
    public Shop shop
    {
        get
        {
            if (_shop == null)
            {
                _shop = FindAnyObjectByType<Shop>(FindObjectsInactive.Include);
            }
            return _shop;
        }
    }
    protected ToppingPlacer _toppingPlacer = null;
    public ToppingPlacer toppingPlacer
    {
        get
        {
            if (_toppingPlacer == null)
            {
                _toppingPlacer = FindAnyObjectByType<ToppingPlacer>(FindObjectsInactive.Include);
            }
            return _toppingPlacer;
        }
    }
    protected ToppingRegistry _toppingRegistery = null;
    public ToppingRegistry toppingRegistery
    {
        get
        {
            if (_toppingRegistery == null)
            {
                _toppingRegistery= FindAnyObjectByType<ToppingRegistry>(FindObjectsInactive.Include);
            }
            return _toppingRegistery;
        }
    }

    protected RoundManager _roundManager = null;
    public RoundManager roundManager
    {
        get
        {
            if (_roundManager == null)
            {
                _roundManager = GetComponent<RoundManager>();
            }
            return _roundManager;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        } else if (_instance != this)
        {
            Debug.LogWarning("LevelManager Instance already set!!! Destroying this new one: " + gameObject.name);
            Destroy(gameObject);
        }
    }

    protected bool _validInstance = true;
    private void OnDestroy()
    {
        _validInstance = false;
        _instance = null;
    }

    public void Initialize(string levelName = "defaultlevel", bool loadSaveData = false)
    {
        if (_saveData == null)
        {
            if (loadSaveData && SaveDataUtility.GetSaveFileNameIfExists(levelName, out string saveFilePath, out string saveFileName))
            {
                int extIndex = saveFileName.IndexOf(SaveDataFileUtility._saveFileExtension);
                while (extIndex >= 0)
                {
                    saveFileName = saveFileName.Substring(0, extIndex) + 
                        (extIndex + SaveDataFileUtility._saveFileExtension.Length < saveFileName.Length 
                            ? saveFileName.Substring(extIndex + SaveDataFileUtility._saveFileExtension.Length) 
                            : "");
                    extIndex = saveFileName.IndexOf(SaveDataFileUtility._saveFileExtension);
                }
                _saveFileName = saveFileName;
                _saveData = SaveDataUtility.LoadSaveData(this.saveFileName, levelName);
            } else
            {
                _saveData = SaveDataUtility.CreateSaveData(this.saveFileName, levelName);
            }
            Debug.Log("Initialized save data to level name: " + levelName + ", file name: " + this.saveFileName);
            // SaveDataUtility.CreateSaveData(levelName)
            //_saveData = 
        }
    }

    public void SaveLevel()
    {
        Debug.Log("Saving level data...");

        // Start: Create the index for the items and toppings
        List<Item> potentialItems = toppingRegistery.allItems;
        List<Topping> potentialToppings = new List<Topping>();
        foreach (Item item in potentialItems)
        {
            Topping topping = item as Topping;
            if (topping != null)
            {
                potentialToppings.Add(topping);
            }
        }
        Dictionary<string, int> toppingIndex = new Dictionary<string, int>();
        Dictionary<string, int> itemIndex = new Dictionary<string, int>();
        for (int i = 0; i < potentialToppings.Count; i++)
        {
            toppingIndex.Add(potentialToppings[i].name, i);
        }
        for (int i = 0; i < potentialItems.Count; i++)
        {
            itemIndex.Add(potentialItems[i].name, i);
        }
        // End: Create the index for the items and toppings

        // Collect all of the towers and toppings, adding them to the data entries
        List<ToppingRegistry.ItemInfo> toppings = toppingRegistery.GetAllPlacedToppings();
        List<DETowerPlaced> allTowers = new List<DETowerPlaced>();
        List<DEItemInventory> allInventory = new List<DEItemInventory>();
        foreach (ToppingRegistry.ItemInfo item in toppings)
        {
            if (item.obj == null) { Debug.LogWarning("Null item in topping registry: " + item.topping); continue; }
            string itemName = UnclonedName(item.topping.name);
            allTowers.Add(new DETowerPlaced("topping" + itemName, toppingIndex[itemName], new DEPosition("pos", item.obj.transform.position, item.obj.transform.rotation.eulerAngles), item.topping.ID.ToString()));
        }
        foreach (Item item in Inventory.inventory.ownedItems)
        {
            string itemName = UnclonedName(item.name);
            allInventory.Add(new DEItemInventory("item" + itemName, itemIndex[itemName], item.ID.ToString()));
        }

        // Create the wrapper data entry items
        DEAllTowers towers = new DEAllTowers("alltowers", allTowers);
        DEAllItemsInventory items = new DEAllItemsInventory("allinventory", allInventory);
        DEIntEntry money = new DEIntEntry("money", Inventory.inventory.Money);
        DEUIntEntry round = new DEUIntEntry("round", roundManager.roundNumber);

        // Set the data entries
        saveData.SetDataEntry(towers, true);
        saveData.SetDataEntry(items, true);
        saveData.SetDataEntry(money, true);
        saveData.SetDataEntry(round, true);

        toppingRegistery.SaveAll(saveData);

        // Use encryptions and write the data
        SaveDataUtility._useEncryptions = _encryptData;
        SaveDataUtility.WriteSaveData(saveData);
        Debug.Log("Done saving level data!");
    }

    public void LoadLevel()
    {
        Debug.Log("Loading level data...");

        // Grab the lists of toppings and items
        List<Item> potentialItems = shop.availableItems;
        List<Topping> potentialToppings = new List<Topping>();
        foreach (Item item in potentialItems)
        {
            Topping topping = item as Topping;
            if (topping != null)
            {
                potentialToppings.Add(topping);
            }
        }

        // Set the money
        if (saveData.TryGetDataEntry("money", out DEIntEntry moneyWrapper))
        {
            Inventory.inventory.Money = moneyWrapper.value;
        }

        if (saveData.TryGetDataEntry("round", out DEUIntEntry roundWrapper))
        {
            roundManager.roundNumber = roundWrapper.value;
        }

        // Place all of the toppings
        if (saveData.TryGetDataEntry("alltowers", out DEAllTowers towerWrapper)) {
            Debug.Log("Read all towers data entry! Placing towers...");
            foreach (DETowerPlaced tower in towerWrapper.towers)
            {
                Topping topping = Instantiate(potentialToppings[tower.towerIndex]); // instantiate it
                topping.name = potentialToppings[tower.towerIndex].name;
                topping.ID = new System.Guid(tower.towerID); // set the GUID
                toppingPlacer.PlaceToppingViaLoad(topping, tower.pos.positionData, Quaternion.Euler(tower.pos.eulers));
            }
        } else
        {
            Debug.Log("Did not find all towers data entry!");
        }

        // Add all of the inventory
        if (saveData.TryGetDataEntry("allinventory", out DEAllItemsInventory itemsWrapper))
        {
            foreach (DEItemInventory item in  itemsWrapper.items)
            {
                Inventory.inventory.AddItem(potentialItems[item.itemIndex], new System.Guid(item.itemID)); // set the GUID
            }
        }

        toppingRegistery.LoadAllToppingData(saveData);

        Debug.Log("Done saving level data!");
    }

    public static string UnclonedName(string name)
    {
        string _cloneText = "(Clone)";
        int index = name.IndexOf(_cloneText);
        while (index >= 0)
        {
            name = name.Substring(0, index) + name.Substring(index + _cloneText.Length);
            index = name.IndexOf(_cloneText);
        }
        return name;
    }
}
