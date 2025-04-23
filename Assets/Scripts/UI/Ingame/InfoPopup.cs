using UnityEngine;
using UnityEngine.EventSystems;
using System.Globalization;
using EventBus;
using System.Linq;

public class InfoPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI description;
    [SerializeField] TMPro.TextMeshProUGUI toppingType;
    [SerializeField] TMPro.TextMeshProUGUI sellPriceText;
    [SerializeField] GameObject sellButton;
    EventSystem eventSystem;
    public bool hovered = false;
    Item item;
    GameObject toppingObj;
    int sellPrice = 0;
    bool isInventoryItem = false;

    void Awake()
    {
        eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
        if (eventSystem == null) { Debug.LogWarning("No event system in scene."); }
    }

    void Start()
    {
        Clear();
    }

    public void SetUp(Item item, GameObject toppingObj)
    {
        isInventoryItem = false;
        this.item = item;
        if (item == null) { return; }
        if (item is Topping topping)
        {
            toppingType.text = ToTitleCase(topping.flags.ToString());
        }

        nameText.text = item.name;
        description.text = item.description;
        sellPrice = item.price / 2;
        sellPriceText.text = "$" + sellPrice;
        this.toppingObj = toppingObj;
        gameObject.SetActive(true);
    }

    public void SetUpForInventoryItem(Item item)
    {
        isInventoryItem = true;
        this.item = item;
        if (item == null) { return; }
        if (item is Topping topping)
        {
            toppingType.text = ToTitleCase(topping.flags.ToString());
        }

        nameText.text = item.name;
        description.text = item.description;
        sellPrice = item.price / 2;
        sellPriceText.text = "$" + sellPrice;
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        nameText.text = "";
        description.text = "";
        sellPriceText.text = "";
        toppingType.text = "";
        item = null;
        toppingObj = null;
        hovered = false;
        if (sellButton == null) { return; }
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (item != null && Input.GetKeyDown(KeyCode.X))
        {
            OnSell();
        }
    }

    public void OnSell()
    {
        Inventory.inventory.Money += sellPrice;
        GameStats.gameStats.moneyEarned += sellPrice;
        GameStats.gameStats.toppingsSold++;
        EventBus<SellEvent>.Raise(new SellEvent(item, toppingObj));

        if (toppingObj != null)
        {
            Destroy(toppingObj);
            Clear();
        }
        if (isInventoryItem)
        {
            if (Inventory.inventory.RemoveItemByID(item.ID) <= 0)
            {
                Clear();
            }
            else 
            {
                Item nextItemInStack = Inventory.inventory.ownedItems.First(x => x.name.Equals(item.name));
                SetUpForInventoryItem(nextItemInStack);
            }
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
