using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ShopObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool hovered = false;
    Item displayItem;

    [SerializeField] Image image;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI priceText;

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
        if (Input.GetMouseButtonDown(0) && hovered)
        {
            Inventory inv = FindFirstObjectByType<Inventory>();
            if (inv.TryBuyItem(displayItem))
            {
                FindFirstObjectByType<InventoryRenderer>().UpdateAllIconPositions();
            }
            
        }
    }

    public void SetUp(Item item)
    {
        image.sprite = item.shopSprite;
        nameText.text = item.name;
        priceText.text = item.price + "";
        displayItem = item;
    }

    public void UpdateInfo()
    {
        image.sprite = displayItem.shopSprite;
        nameText.text = displayItem.name;
        priceText.text = displayItem.price + "";
    }
}
