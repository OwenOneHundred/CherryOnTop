using System.Collections.Generic;
using System.Linq;
using EventBus;
using Unity.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    [SerializeField] List<Item> startingInventoryItems;
    private void Awake()
    {
        if (inventory == null)
        {
            inventory = this;
        }
        else if (inventory != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public InventoryRenderer inventoryRenderer;

    public InventoryEffectManager inventoryEffectManager;
    public IngameUI ingameUI;
    void Start()
    {
        inventoryEffectManager = GetComponent<InventoryEffectManager>();
        ingameUI = GameObject.FindAnyObjectByType<IngameUI>();

        foreach (Item item in startingInventoryItems) // add starting items to inventory display
        {
            inventoryRenderer.AddItemToDisplay(item);
        }
    }

    List<Item> ownedItems = new List<Item>();

    int money = 0;
    public int Money
    {
        get { return money; }
        set
        {
            if (value > money) // if gaining money, apply money limit
            {
                money = inventoryEffectManager.ApplyLimit<LimitMoney>(value);
            }
            else 
            {
                money = value;
            }
            ingameUI.SetMoney(value);
        }
    }
    int cakePoints = 0;
    public int CakePoints
    {
        get { return cakePoints; }
        set
        {
            cakePoints = value;
            ingameUI.SetCakeScore(value);
        }
    }
    
    public bool TryBuyItem(Item item)
    {
        if (item.price > money) { Debug.Log(money + " " + item.price);  return false; } // can't afford
        if (0 > inventoryEffectManager.GetLimit<LimitBuying>()) { return false; } // TODO replace 0 with shop manager purchases count 

        AddItem(item);
        Money -= item.price;
        inventoryRenderer.UpdateAllIconPositions();
        EventBus<BuyEvent>.Raise(new BuyEvent(item));
        return true;
    }

    public void AddItem(Item item)
    {
        ownedItems.Add(item);
        inventoryRenderer.AddItemToDisplay(item);
    }

    public void RemoveItem(Item item)
    {
        ownedItems.Remove(item);
    }
}
