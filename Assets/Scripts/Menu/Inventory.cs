using System;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using Unity.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    [SerializeField] int initialMoney = 20;
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
    [SerializeField] float minTimeBetweenMoneyChanges = 0.25f;

    public InventoryRenderer inventoryRenderer;

    public InventoryEffectManager inventoryEffectManager;
    public IngameUI ingameUI;
    void Start()
    {
        Money = initialMoney;

        inventoryEffectManager = GetComponent<InventoryEffectManager>();
        ingameUI = GameObject.FindAnyObjectByType<IngameUI>();

        foreach (Item item in startingInventoryItems) // add starting items to inventory display
        {
            AddItem(item);
        }
    }

    List<Item> ownedItems = new List<Item>();

    /// <summary>
    /// Money is not changed directly, because the animation has play first.
    /// This int is only changed by ManageBufferedMoneyChanges. Changing the money value adds a buffered, money change
    /// which is applied in update. This way, any two money changes on the same frame do not consider each others' impact.
    /// </summary>
    int money = 0;
    public int Money
    {
        get { return money; }
        set
        {
            int limitedValue = inventoryEffectManager.ApplyLimit<LimitMoney>(value);
            int change = limitedValue - money;
            BufferMoneyChange(change);
        }
    }
    List<int> bufferedMoneyChanges = new();

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
    [SerializeField] MoneyChangeDisplay moneyChangeDisplay;

    public bool TryBuyItem(Item item)
    {
        if (item.price > money) { Debug.Log("only have " + money + ", this costs " + item.price);  return false; } // can't afford
        if (0 > inventoryEffectManager.GetLimit<LimitBuying>()) { return false; } // TODO replace 0 with shop manager purchases count 

        Money -= item.price;
        EventBus<BuyEvent>.Raise(new BuyEvent(item));

        AddItem(item);
        return true;
    }

    public void AddItem(Item item)
    {
        item = Instantiate(item); // Item SOs are currently instantiated here, when added to inventory.

        ownedItems.Add(item);
        inventoryRenderer.AddItemToDisplay(item);
        item.Initialize();
    }

    public void RemoveItem(Item item)
    {
        ownedItems.Remove(item);
        inventoryRenderer.RemoveItemFromDisplay(item);
    }

    float moneyChangeTimer = 0;

    public void Update()
    {
        moneyChangeTimer += Time.deltaTime;
        ManageBufferedMoneyChanges();
    }

    private void ManageBufferedMoneyChanges()
    {
        if (moneyChangeTimer > minTimeBetweenMoneyChanges)
        {
            if (bufferedMoneyChanges.Count != 0)
            {
                foreach (int moneyChange in bufferedMoneyChanges)
                {
                    ApplyMoneyChange(moneyChange);
                }
                bufferedMoneyChanges.Clear();
                ingameUI.SetMoney(Money);
            }
            moneyChangeTimer = 0;
        }
    }

    private void ApplyMoneyChange(int moneyChange)
    {
        money += moneyChange;
        moneyChangeDisplay.AddToDisplay(moneyChange);
    }

    private void BufferMoneyChange(int change)
    {
        bufferedMoneyChanges.Add(change);
    }
}
