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
            addToInventory();
        }
    }

    public void addToInventory()
    {
        InventoryRenderer inventory = FindFirstObjectByType<InventoryRenderer>();
        inventory.AddItemToDisplay(displayItem);
        inventory.UpdateAllIcons();
    }

    public void SetUp(Item item)
    {
        image.sprite = item.shopSprite;
        nameText.text = item.name;
        priceText.text = item.price + "";
    }
}
