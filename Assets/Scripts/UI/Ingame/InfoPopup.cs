using UnityEngine;
using UnityEngine.EventSystems;
using System.Globalization;
using EventBus;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class InfoPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI description;
    [SerializeField] TMPro.TextMeshProUGUI toppingType;
    [SerializeField] TMPro.TextMeshProUGUI sellPriceText;
    [SerializeField] GameObject sellButton;
    [SerializeField] GameObject backObject;
    [SerializeField] List<TMPro.TextMeshProUGUI> stats;
    [SerializeField] TMPro.TextMeshProUGUI specialInfo;
    [SerializeField] Image backIcon;
    EventSystem eventSystem;
    public bool hovered = false;
    Item item;
    GameObject toppingObj;
    int sellPrice = 0;
    bool isInventoryItem = false;
    bool _flipped = false;
    bool Flipped
    {
        get { return _flipped; }
        set
        {
            if (_flipped == value || flipping) { return; }
            if (gameObject.activeInHierarchy) { StartCoroutine(Flip(value)); }
            else { _flipped = false; backObject.SetActive(value); }
        }
    }
    bool flipping = false;

    private System.Collections.IEnumerator Flip(bool flipped)
    {
        flipping = true;
        while (transform.localScale.x > 0.05f)
        {
            transform.localScale -= new Vector3(14 * Time.deltaTime, 0, 0);
            yield return null;
        }
        backObject.SetActive(flipped);
        SetUpBack();
        while (transform.localScale.x < 1f)
        {
            transform.localScale += new Vector3(14 * Time.deltaTime, 0, 0);
            yield return null;
        }
        transform.localScale = Vector3.one;

        _flipped = flipped;
        flipping = false;
    }

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
        sellPrice = item.SellPrice;
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
        sellPrice = item.SellPrice;
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
        flipping = false;
        Flipped = false;
        StopAllCoroutines();
        transform.localScale = Vector3.one;
    }

    public void OnClickI()
    {
        Flipped = !Flipped;
    }

    private void SetUpBack()
    {
        if (item is Topping topping)
        {
            AttackManager attackManager = GetAttackManager();
            if (attackManager == null) // no attack on object
            {
                SetAllBlank();
            }
            else
            {
                if (toppingObj == null) // there's an attack on the object, but it's in your inventory
                {
                    stats[0].text = "Damage: " + attackManager.attackTemplate.GetVisibleDamage();
                    stats[1].text = "Cooldown: " + attackManager.attackTemplate.cooldown.ToString("F2");
                    stats[2].text = "Range: " + attackManager.GetComponent<TargetingSystem>().GetRange().ToString("F2");
                    stats[3].text = "Pierce: " + attackManager.attackTemplate.GetPierce();
                    stats[4].text = "Cake Points: " + topping.cakePoints;
                }
                else // there's an attack on the object, and it's placed on the cake
                {
                    stats[0].text = "Damage: " + attackManager.GetAttack().GetVisibleDamage();
                    stats[1].text = "Cooldown: " + attackManager.GetAttack().cooldown.ToString("F2");
                    stats[2].text = "Range: " + attackManager.GetComponent<TargetingSystem>().GetRange().ToString("F2");
                    stats[3].text = "Pierce: " + attackManager.GetAttack().GetPierce();
                    stats[4].text = "Cake Points: " + topping.cakePoints;
                }
            }

            backIcon.sprite = topping.shopSprite;
            specialInfo.text = topping.GetSpecialInfo();
        }

        AttackManager GetAttackManager()
        {
            return toppingObj == null ? topping.towerPrefab.GetComponentInChildren<AttackManager>() : toppingObj.GetComponentInChildren<AttackManager>();
        }

        void SetAllBlank()
        {
            stats[0].text = "Damage: -";
            stats[1].text = "Cooldown: -";
            stats[2].text = "Range: -";
            stats[3].text = "Pierce: -";
            stats[4].text = "Cake Points: 0";
        }
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
        if (toppingObj != null)
        {
            Shop.shop.SellItemOffCake(item, toppingObj);
            Clear();
        }
        if (isInventoryItem)
        {
            if (Shop.shop.SellItemFromInventory(item) <= 0)
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
