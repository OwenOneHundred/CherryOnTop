using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public float speed = 10;
    bool open;
    bool moving;
    readonly float closedPos = -1780;
    readonly float openPos = -230;
    RectTransform rect;

    public int columns = 3;
    public int rows = 2;
    public int totalItems = 6;
    [SerializeField] int iconSpacing = 100;

    [SerializeField] GameObject shopObjPrefab;
    public List<Item> currentItems = new();
    public List<Item> availableItems = new();
    [SerializeField] Transform itemParent;
    [SerializeField] TMPro.TextMeshProUGUI rerollsText;
    [SerializeField] TMPro.TextMeshProUGUI rerollButtonText;
    [SerializeField] Button rerollButton;
    [SerializeField] GameObject sellParticleEffect;
    public List<ShopObj> shopObjs = new();
    int rerolls = 0;
    public int Rerolls
    {
        get { return rerolls; }
        set
        {
            UpdateRerollsText(value);
            rerolls = value;
        }
    }

    private void UpdateRerollsText(int freeRerolls)
    {
        rerollsText.text = "Free rerolls: " + freeRerolls;
        rerollButtonText.text = (freeRerolls > 0) ? "Free\nReroll" : "Reroll\n$" + LiveRerollPrice;
    }

    [SerializeField] private int _baseRerollPrice = 2;
    private int _liveRerollPrice = 2;
    public int LiveRerollPrice
    {
        get { return _liveRerollPrice; }
        set
        {
            _liveRerollPrice = value;
            UpdateRerollsText(Rerolls);
        }
    }

    [SerializeField] AudioFile error;
    [SerializeField] AudioFile openShop;
    [SerializeField] AudioFile closeShop;
    [SerializeField] AudioFile rerollSound;
    [SerializeField] AudioFile shopButtonSound;
    public AudioFile onRollRare;
    public InfoPopup infoPopup; // shop probably shouldn't have this but it needs to be cached so whatever

    public Item mostRecentlyBoughtItem { get; set; }
    public int purchasesThisRound = 0;

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
        Rerolls = Rerolls;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ToggleOpen();
        }
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
            //SoundEffectManager.sfxmanager.PlayOneShot(shopButtonSound);

            open = value;
            moving = true;

            UpdateAllIconText();

            StartCoroutine(Mover());
        }
    }

    private bool _doRerollPriceIncrease = true;
    public bool DoRerollPriceIncrease
    {
        get { return _doRerollPriceIncrease; }
        set { _doRerollPriceIncrease = DoRerollPriceIncrease; }
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
    }

    public void OnClickReroll()
    {
        if (Rerolls > 0)
        {
            Rerolls -= 1;
            EventBus<RerollEvent>.Raise(new RerollEvent());
            SoundEffectManager.sfxmanager.PlayOneShot(rerollSound);
            RerollItems();
            PlayRerollAnim();
        }
        else if (Inventory.inventory.Money >= LiveRerollPrice)
        {
            Inventory.inventory.Money -= LiveRerollPrice;
            EventBus<RerollEvent>.Raise(new RerollEvent());
            SoundEffectManager.sfxmanager.PlayOneShot(rerollSound);
            RerollItems();
            PlayRerollAnim();
            if (DoRerollPriceIncrease) { LiveRerollPrice += 1; }
        }
        else
        {
            SoundEffectManager.sfxmanager.PlayOneShot(error);
        }
    }

    public void OnRoundEnd()
    {
        //RerollItems();
        purchasesThisRound = 0;
        LiveRerollPrice = _baseRerollPrice;
    }

    public void PlayRerollAnim()
    {
        float iconAppearDelay = 0.0125f;
        for (int i = 0; i < shopObjs.Count; i++)
        {
            StartCoroutine(shopObjs[i].IconAppearAnim(i * iconAppearDelay));
        }
    }

    public void RerollItems()
    {
        currentItems.Clear();
        PopulateShop();
        UpdateAllIcons();
        UpdateAllIconText();
    }

    public void PopulateShop()
    {
        // Create list of weights
        List<float> weights = new();
        foreach (Item item in availableItems) { weights.Add(item.rarity.GetWeight()); }

        // Populate the shop using the weights
        for (int i = 0; i < totalItems; i++)
        {
            int item = GeneralUtil.RandomWeighted(weights);
            currentItems.Add(availableItems[item]);
        }
    }

    public void UpdateRerollButtonFadedness()
    {
        rerollButton.interactable = Inventory.inventory.Money >= LiveRerollPrice || Rerolls > 0;
    }

    public void UpdateAllIcons()
    {
        foreach (ShopObj shopObj in shopObjs) Destroy(shopObj.gameObject);
        shopObjs.Clear();
        for (int i = 0; i < currentItems.Count; i++)
        {
            GameObject newIcon = Instantiate(shopObjPrefab, itemParent);
            ShopObj shopObj = newIcon.GetComponent<ShopObj>();
            shopObjs.Add(shopObj);
            shopObj.SetUp(currentItems[i]);
            newIcon.GetComponent<RectTransform>().anchoredPosition +=
                new Vector2((i % columns), (int)(-i / rows)) * iconSpacing;
        }
    }

    public void UpdateAllIconText()
    {
        foreach (ShopObj shopObj in shopObjs)
        {
            shopObj.UpdateInfo();
        }
    }

    public void SellItemOffCake(Item item, GameObject toppingObj)
    {
        StartCoroutine(SellAnimation(toppingObj));

        Inventory.inventory.Money += item.SellPrice;
        GameStats.gameStats.moneyEarned += item.SellPrice;
        GameStats.gameStats.toppingsSold++;
        EventBus<SellEvent>.Raise(new SellEvent(item, toppingObj));

        // might need to clear infopopup if it is displaying an item that was just sold by something other than the player
    }

    private IEnumerator SellAnimation(GameObject toppingObj)
    {
        float shrinkSpeed = 10;
        if (toppingObj == null) { yield break; }

        while (toppingObj.transform.localScale.x > 0.001f)
        {
            toppingObj.transform.localScale -= shrinkSpeed * Time.deltaTime * Vector3.one;
            yield return null;
        }

        Instantiate(sellParticleEffect, toppingObj.transform.position, Quaternion.identity);

        Destroy(toppingObj);
    }

    /// <summary>
    /// Sell item from inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>Number of items left in the stack</returns>
    public int SellItemFromInventory(Item item)
    {
        Inventory.inventory.Money += item.SellPrice;
        GameStats.gameStats.moneyEarned += item.SellPrice;
        GameStats.gameStats.toppingsSold++;
        EventBus<SellEvent>.Raise(new SellEvent(item, null));

        return Inventory.inventory.RemoveItemByID(item.ID);
    }
}
