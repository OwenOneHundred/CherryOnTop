using System.Collections.Generic;
using GameSaves;
using NUnit.Framework;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    protected LevelManager _instance;
    public LevelManager Instance
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

    [SerializeField] protected string _saveFileName = "levelsave";
    protected SaveData _saveData = null;
    public SaveData saveData
    {
        get
        {
            if (_saveData == null)
            {
                _saveData = SaveData.LoadData(_saveFileName);
            }
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

    public void SaveLevel()
    {
        // TODO
        // loop through currently placed toppings in level
        // and add them as DETowerPlaced to the DEAllTowers save data entry
        List<ToppingRegistry.ItemInfo> toppings = toppingRegistery.GetAllPlacedToppings();
        //List<Topping> potentialToppings = shop.availableItems;
        Dictionary<string, int> toppingIndex = new Dictionary<string, int>();
        
        foreach (ToppingRegistry.ItemInfo item in toppings)
        {

            //item.topping
        }
    }

    public void LoadLevel()
    {
        if (saveData.TryGetData("alltowers", out DEAllTowers towerWrapper)) {
            foreach (DETowerPlaced tower in towerWrapper.towers)
            {
                // TODO
                // place topping using tower data
                // Topping topping = shop.availableItems[tower.towerIndex];
                // toppingPlacer.PlaceTopping(topping, pos.positionData, pos.eulers, pos.scale);
            }
        }
    }
}
