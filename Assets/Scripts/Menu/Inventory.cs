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

    public InventoryRenderer inventoryRenderer;

    public InventoryEffectManager inventoryEffectManager;
    void Start()
    {
        inventoryEffectManager = GetComponent<InventoryEffectManager>();
    }

    List<Item> ownedItems = new List<Item>();

    int money = 69;
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
        if (item.price > money) { Debug.Log(money + " " + item.price);  return false; } // can't afford
        if (0 > inventoryEffectManager.GetLimit<LimitBuying>()) { return false; } // TODO replace 0 with shop manager purchases count 

        AddItem(item);
        Money -= item.price;
        inventoryRenderer.UpdateAllIcons();
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
