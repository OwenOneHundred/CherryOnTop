using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryIconControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Topping assignedTopping;
    bool hovered = false;
    bool selected = false;
    public bool beingPlaced = false;
    [SerializeField] Image image;
    [SerializeField] TMPro.TextMeshProUGUI number;
    [SerializeField] GameObject outline;
    InfoPopup infoPopup;
    int amountInStack = 1;
    public int AmountInStack
    {
        get { return amountInStack; }
        set 
        {
            number.text = value == 1 ? "" : value + "";
            amountInStack = value;
        }
    }

    private void Awake()
    {
        infoPopup = Shop.shop.infoPopup;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!hovered && !infoPopup.hovered)
            {
                OnClickedOff();
            }
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (hovered)
            {
                OnClicked();
            }
        }
    }

    private void StartPlacing()
    {
        beingPlaced = true;
        image.color = Color.gray;
    }

    public void StopPlacing() // called by topping placer
    {
        if (image == null) { return; }
        beingPlaced = false;
        image.color = Color.white;
    }

    public void OnClicked()
    {
        selected = true;
        ToppingPlacer.toppingPlacer.StartPlacingTopping(assignedTopping, this);
        StartPlacing();
        infoPopup.SetUpForInventoryItem(assignedTopping);
        outline.SetActive(true);
    }

    public void OnClickedOff()
    {
        if (!selected) { return; }
        infoPopup.Clear();
        selected = false;
        outline.SetActive(false);
    }

    public void OnDestroy()
    {
        if (!selected) { return; }
        selected = false;
        outline.SetActive(false);
    }

    public void SetUp(Sprite sprite)
    {
        image.sprite = sprite;
        number.text = "";
    }
}
