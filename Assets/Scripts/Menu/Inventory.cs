using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    private void Awake()
    {
        if (inventory == null)
        {
            inventory = this;
        }
        else if (inventory != this)
        {
            Destroy(gameObject);
        }
    }

    public InventoryEffectManager inventoryEffectManager;
    void Start()
    {
        inventoryEffectManager = GetComponent<InventoryEffectManager>();
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
        }
    }
    int cakePoints = 0;
    public int CakePoints
    {
        get { return cakePoints; }
        set { cakePoints = value; }
    }
    
    public bool TryBuyItem(Item item)
    {
        if (item.price > money) { return false; } // can't afford
        if (0 > inventoryEffectManager.GetLimit<LimitBuying>()) { return false; } // TODO replace 0 with shop manager purchases count 

        AddItem(item);
        Money -= item.price;
        return true;
    }

    public void AddItem(Item item)
    {
        ownedItems.Add(item);
    }

    public void RemoveItem(Item item)
    {
        ownedItems.Remove(item);
    }
}
