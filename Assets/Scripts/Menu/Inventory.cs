using System;
using System.Collections.Generic;
using System.Linq;
using EventBus;
using Unity.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory inventory;
    public int initialMoney = 15;
    [SerializeField] float baseTimeBetweenMoneyChanges = 0.35f;
    float scalingMoneyGainTime = 0;
    float moneyGainPitch = 1;
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

        scalingMoneyGainTime = baseTimeBetweenMoneyChanges;
    }

    public InventoryRenderer inventoryRenderer;

    public InventoryEffectManager inventoryEffectManager;
    public IngameUI ingameUI;
    [SerializeField] AudioFile getMoneySFX;
    [SerializeField] AudioFile buySFX;
    [SerializeField] AudioFile error;
    void Start()
    {
        if (!LevelManager.levelWasLoadedFromSave)
        {
            Money = initialMoney;
        }

        inventoryEffectManager = GetComponent<InventoryEffectManager>();
        ingameUI = GameObject.FindAnyObjectByType<IngameUI>();

        foreach (Item item in startingInventoryItems) // add starting items to inventory display
        {
            AddItem(item);
        }
    }

    public List<Item> ownedItems = new List<Item>();

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
            if (change > 0)
            {
                BufferMoneyChange(change);
            }
            else 
            {
                ApplyMoneyChange(change);
            }
        }
    }
    List<int> bufferedMoneyChanges = new();
    
    [SerializeField] MoneyChangeDisplay moneyChangeDisplay;

    public bool TryBuyItem(Item item)
    {
        if (item.price > money)
        {
            SoundEffectManager.sfxmanager.PlayOneShot(error);
            return false;
        } 

        if (0 > inventoryEffectManager.GetLimit<LimitBuying>()) { return false; } // TODO replace 0 with shop manager purchases count 

        Money -= item.price;
        EventBus<BuyEvent>.Raise(new BuyEvent(item));
        SoundEffectManager.sfxmanager.PlayOneShot(buySFX);

        AddItem(item);

        Shop.shop.mostRecentlyBoughtItem = item;
        return true;
    }

    public void GetItemForFree(Item item)
    {
        SoundEffectManager.sfxmanager.PlayOneShot(buySFX);

        AddItem(item);
    }

    public void AddItem(Item template, Guid id = default)
    {
        Item item = Instantiate(template); // Item SOs are currently instantiated here, when added to inventory.
        item.SetUpEffectsAndWhen(); // Item SOs' effects are instantiated here.
        item.name = template.name;
        
        if (id == default)
        {
            item.ID = Guid.NewGuid();
        }
        else
        {
            item.ID = id;
        }

        ownedItems.Add(item);
        inventoryRenderer.AddItemToDisplay(item);
    }

    public int RemoveOneOfItem(Item item)
    {
        ownedItems.Remove(item);
        return inventoryRenderer.RemoveOneFromItemFromDisplay(item); 
    }

    float moneyChangeTimer = 0;

    public void Update()
    {
        moneyChangeTimer += Time.deltaTime;
        ManageBufferedMoneyChanges();
    }

    private void ManageBufferedMoneyChanges()
    {
        if (moneyChangeTimer > scalingMoneyGainTime)
        {
            if (bufferedMoneyChanges.Count != 0)
            {
                ApplyMoneyChange(bufferedMoneyChanges[0]);
                bufferedMoneyChanges.RemoveAt(0);
                scalingMoneyGainTime = Mathf.Clamp(scalingMoneyGainTime - 0.02f, 0.02f, 1);
                moneyGainPitch = Mathf.Clamp(moneyGainPitch + 0.04f, 1, 2.5f);
            }
            else
            {
                ResetMoneyGainScaling();
            }
            moneyChangeTimer = 0;
        }
    }

    private void ResetMoneyGainScaling()
    {
        scalingMoneyGainTime = baseTimeBetweenMoneyChanges;
        moneyGainPitch = 1;
    }

    private void ApplyMoneyChange(int moneyChange)
    {
        money += moneyChange;
        moneyChangeDisplay.AddToDisplay(moneyChange);
        ingameUI.SetMoney(Money);
        if (moneyChange > 0)
        {
            SoundEffectManager.sfxmanager.PlayOneShotWithPitch(getMoneySFX, moneyGainPitch);
        }

        Shop.shop.UpdateRerollButtonFadedness();
    }

    private void BufferMoneyChange(int change)
    {
        bufferedMoneyChanges.Add(change);
    }
}
