using System.Collections.Generic;
using GameSaves;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] protected string _saveFileName = "levelsave";
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
            if (loadSaveData)
            {
                _saveData = SaveDataUtility.LoadSaveData(_saveFileName, levelName);
            } else
            {
                _saveData = SaveDataUtility.CreateSaveData(_saveFileName, levelName);
            }
            // SaveDataUtility.CreateSaveData(levelName)
            //_saveData = 
        }
    }

    public void SaveLevel()
    {
        Debug.Log("Saving level data...");

        // Start: Create the index for the items and toppings
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

        // Set the data entries
        saveData.SetDataEntry(towers, true);
        saveData.SetDataEntry(items, true);
        saveData.SetDataEntry(money, true);

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

        // Place all of the toppings
        if (saveData.TryGetDataEntry("alltowers", out DEAllTowers towerWrapper)) {
            foreach (DETowerPlaced tower in towerWrapper.towers)
            {
                Topping topping = Instantiate(potentialToppings[tower.towerIndex]); // instantiate it
                topping.ID = new System.Guid(tower.towerID); // set the GUID
                toppingPlacer.PlaceTopping(topping, tower.pos.positionData, Quaternion.Euler(tower.pos.eulers));
            }
        }

        // Add all of the inventory
        if (saveData.TryGetDataEntry("allinventory", out DEAllItemsInventory itemsWrapper))
        {
            foreach (DEItemInventory item in  itemsWrapper.items)
            {
                Inventory.inventory.AddItem(potentialItems[item.itemIndex], new System.Guid(item.itemID)); // set the GUID
            }
        }

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
