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
    readonly float timeBetweenGainMoney = 0.1f;
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
            GameStats.gameStats.moneyEarned += initialMoney;
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
        if (item.price > money || Shop.shop.purchasesThisRound >= inventoryEffectManager.GetLimit<LimitBuying>())
        {
            SoundEffectManager.sfxmanager.PlayOneShot(error);
            return false;
        } 

        Money -= item.price;
        EventBus<BuyEvent>.Raise(new BuyEvent(item));
        SoundEffectManager.sfxmanager.PlayOneShot(buySFX);
        Shop.shop.purchasesThisRound += 1;

        AddItem(item);

        Shop.shop.mostRecentlyBoughtItem = item;
        GameStats.gameStats.toppingsBought++;
        GameStats.gameStats.moneySpent += item.price;
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

    /// <summary>
    /// Remove item from inventory by ID. Returns the number of items left in that stack after operation.
    /// </summary>
    /// <param name="id">ID of item to remove</param>
    /// <returns>Number of items left in the stack</returns>
    public int RemoveItemByID(Guid id)
    {
        int index = ownedItems.FindIndex(x => x.ID.Equals(id));
        if (index >= 0)
        {
            Item item = ownedItems[index];
            ownedItems.RemoveAt(index);
            Item replacementItem;
            try
            {
                replacementItem = ownedItems.First(x => x.name.Equals(item.name));
            }
            catch
            {
                replacementItem = null;
            }
            return inventoryRenderer.RemoveOneByIDFromDisplay(item, replacementItem);
        }
        return 0;
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
                scalingMoneyGainTime = Mathf.Clamp(scalingMoneyGainTime - 0.015f, timeBetweenGainMoney, 1);
                moneyGainPitch = Mathf.Clamp(moneyGainPitch + 0.04f, 1, 2.4f);
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

        Shop.shop.UpdateAllIconText();
        Shop.shop.UpdateRerollButtonFadedness();
    }

    private void BufferMoneyChange(int change)
    {
        bufferedMoneyChanges.Add(change);
    }

    readonly int yarnAmountMultiplier = 4;
    public int GetStackCount()
    {
        return ownedItems.Select(x => x.name).Distinct().Count();
    }

    public int GetBiggestStack(bool countYarnMultiplier)
    {
        if (ownedItems.Count == 0) { return 0; }
        Dictionary<string, int> values = new();

        foreach (Item item in ownedItems)
        {
            int increaseAmount = 1;
            if (countYarnMultiplier && item.name == "Yarn") { increaseAmount = yarnAmountMultiplier; }
            if (values.ContainsKey(item.name))
            {
                values[item.name] += increaseAmount;
                Debug.Log(values[item.name]);
            }
            else
            {
                values.Add(item.name, increaseAmount);
            }
        }
        return values.Select(x => x.Value).Max();
    }

    public int GetInventoryCount(bool countYarnMultiplier)
    {
        int yarnAmount = countYarnMultiplier ? ownedItems.Select(x => x.name == "Yarn").Count() * (yarnAmountMultiplier - 1) : 0;
        return ownedItems.Count + yarnAmount;
    }
}
