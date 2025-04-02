using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    public float speed = 10;
    bool open;
    bool moving;
    readonly float closedPos = -1780;
    readonly float openPos = -230;
    RectTransform rect;

    [SerializeField] int columns = 3;
    [SerializeField] int iconSpacing = 100;
    [SerializeField] int totalItems = 4;

    [SerializeField] GameObject shopObjPrefab;
    public List<Item> currentItems = new();
    public List<Item> availableItems = new();
    [SerializeField] Transform itemParent;
    [SerializeField] TMPro.TextMeshProUGUI rerollsText;
    public List<ShopObj> shopObjs = new();
    int rerolls = 0;
    public int Rerolls
    {
        get { return rerolls; }
        set
        { 
            rerollsText.text = "Free rerolls: " + value;
            rerolls = value;
        }
    }
    public int rerollPrice = 4;
    [SerializeField] AudioFile error;
    [SerializeField] AudioFile openShop;
    [SerializeField] AudioFile closeShop;
    [SerializeField] AudioFile rerollSound;

    public Item mostRecentlyBoughtItem { get; set; }

    public static Shop shop;
    public ShopInfoPanel shopInfoPanel;
    public void Awake()
    {
        if (shop == this || shop == null) { shop = this; }
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();
        RerollItems();
    }

    public void ToggleOpen()
    {
        Open = !Open;
    }

    public bool Open
    {
        get { return open; }
        private set
        {
            if (open == value) { return; }
            if (moving) { return; }

            SoundEffectManager.sfxmanager.PlayOneShot(!open ? openShop : closeShop);

            open = value;
            moving = true;

            StartCoroutine(Mover());
        }
    }

    public IEnumerator Mover()
    {
        float goal = open ? openPos : closedPos;

        while (Mathf.Abs(rect.anchoredPosition.x - goal) >= 0.01f)
        {
            rect.anchoredPosition =
                Vector2.MoveTowards(rect.anchoredPosition, new Vector2(goal, rect.anchoredPosition.y), speed * Time.deltaTime);
            yield return null;
        }
        rect.anchoredPosition = new Vector2(goal, rect.anchoredPosition.y);

        moving = false;

        //UpdateAllIcons();
    }

    public void OnClickReroll()
    {
        if (Rerolls > 0)
        {
            Rerolls -= 1;
            EventBus<RerollEvent>.Raise(new RerollEvent());
            SoundEffectManager.sfxmanager.PlayOneShot(rerollSound);
            RerollItems();
        }
        else if (Inventory.inventory.Money >= rerollPrice)
        {
            Inventory.inventory.Money -= rerollPrice;
            EventBus<RerollEvent>.Raise(new RerollEvent());
            SoundEffectManager.sfxmanager.PlayOneShot(rerollSound);
            RerollItems();
        }
        else 
        {
            SoundEffectManager.sfxmanager.PlayOneShot(error);
        }
    }

    public void OnRoundEnd()
    {
        RerollItems();
    }

    public void RerollItems()
    {
        currentItems.Clear();
        PopulateShop();
        UpdateAllIcons();
    }

    public void PopulateShop()
    {
        for (int i = 0; i < totalItems; i++)
        {
            int item = Random.Range(0, availableItems.Count);
            currentItems.Add(availableItems[item]);
        }
    }

    public void UpdateAllIcons() // TODO: this function spawns copies of icons on top of each other when shop is opened and closed
    {
        // Also resets the purchase status of shop items
        // Not sure if this needs to be fixed
        foreach (ShopObj shopObj in shopObjs) Destroy(shopObj.gameObject);
        shopObjs.Clear();
        for (int i = 0; i < currentItems.Count; i++) {
            GameObject newIcon = Instantiate(shopObjPrefab, itemParent);
            ShopObj shopObj = newIcon.GetComponent<ShopObj>();
            shopObjs.Add(shopObj);
            shopObj.SetUp(currentItems[i]);
            newIcon.GetComponent<RectTransform>().anchoredPosition +=
                new Vector2((i % columns), (int) (-i / columns)) * iconSpacing;
        }
    }

    public void UpdateAllIconText()
    {
        List<ShopObj> shopObjsCopy = new(shopObjs);
        foreach (ShopObj shopObj in shopObjsCopy)
        {
            // hack to manage shopobj list. would be better if shop icons were better tracked, removable, etc
            if (shopObj == null) { shopObjs.Remove(shopObj); continue; }
            
            shopObj.UpdateInfo();
        }
    }
}
