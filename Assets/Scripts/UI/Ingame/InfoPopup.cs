using UnityEngine;
using UnityEngine.EventSystems;

public class InfoPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI itemType;
    [SerializeField] TMPro.TextMeshProUGUI description;
    [SerializeField] TMPro.TextMeshProUGUI toppingType;
    [SerializeField] TMPro.TextMeshProUGUI sellPrice;
    EventSystem eventSystem;
    bool hovered = false;
    bool isFirstFrame = true;
    Item item;
    GameObject toppingObj;

    void Awake()
    {
        eventSystem = GameObject.FindAnyObjectByType<EventSystem>();
        if (eventSystem == null) { Debug.LogWarning("No event system in scene."); }
    }
    public void SetUp(Item item, GameObject toppingObj)
    {
        this.item = item;
        this.toppingObj = toppingObj;
        if (item == null) { return; }
        if (item is Topping topping)
        {
            toppingType.text = topping.flags.ToString();
            itemType.text = "Topping";
        }
        else
        {
            itemType.text = "Ingredient";
        }

        nameText.text = item.name;
        description.text = item.description;
        sellPrice.text = "Sell " + (item.price / 2);
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
        Destroy(gameObject);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       hovered = true;
    }
}
