using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Globalization;
using UnityEditor.PackageManager;
using EventBus;

public class InfoPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI itemType;
    [SerializeField] TMPro.TextMeshProUGUI description;
    [SerializeField] TMPro.TextMeshProUGUI toppingType;
    [SerializeField] TMPro.TextMeshProUGUI sellPriceText;
    EventSystem eventSystem;
    bool hovered = false;
    bool isFirstFrame = true;
    Item item;
    GameObject toppingObj;
    int sellPrice = 0;
    bool isInventoryItem = false;

    void Awake()
    {
        eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
        if (eventSystem == null) { Debug.LogWarning("No event system in scene."); }
    }
    public void SetUp(Item item, GameObject toppingObj)
    {
        isInventoryItem = false;
        this.item = item;
        if (item == null) { return; }
        if (item is Topping topping)
        {
            toppingType.text = ToTitleCase(topping.flags.ToString());
            itemType.text = "Topping";
        }
        else
        {
            itemType.text = "Ingredient";
        }

        nameText.text = item.name.Replace("(Clone)", "");
        description.text = item.description;
        sellPrice = item.price / 2;
        sellPriceText.text = "Sell: $" + sellPrice;
        this.toppingObj = toppingObj;
    }

    public void SetUpForInventoryItem(Item item)
    {
        isInventoryItem = true;
        this.item = item;
        if (item == null) { return; }
        if (item is Topping topping)
        {
            toppingType.text = ToTitleCase(topping.flags.ToString());
            itemType.text = "Topping";
        }
        else
        {
            itemType.text = "Ingredient";
        }

        nameText.text = item.name.Replace("(Clone)", "");
        description.text = item.description;
        sellPrice = item.price / 2;
        sellPriceText.text = "Sell: $" + sellPrice;
    }

    public void Clear()
    {
        nameText.text = "";
        description.text = "";
        sellPriceText.text = "";
        itemType.text = "";
        toppingType.text = "";
        item = null;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFirstFrame)
        {
            if (!hovered)
            {
                OnClickedOff();
            }
        }
        isFirstFrame = false;
    }

    private void OnClickedOff()
    {
        Destroy(gameObject);
    }

    public void OnSell()
    {
        Inventory.inventory.Money += sellPrice;
        EventBus<SellEvent>.Raise(new SellEvent(item, toppingObj));
        item.DeregisterEffects();

        Destroy(gameObject);
        if (toppingObj != null)
        {
            Destroy(toppingObj);
        }
        if (isInventoryItem)
        {
            Inventory.inventory.RemoveItem(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       hovered = true;
    }

    public static string ToTitleCase(string title)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(title.ToLower()); 
    }
}
