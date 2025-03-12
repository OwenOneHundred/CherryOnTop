using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{
    [SerializeField] GameObject iconPrefab;
    [SerializeField] Slider slider;
    [SerializeField] int columns = 2;
    [SerializeField] Vector2 iconStartPos;
    [SerializeField] float bottomDistance = 488;
    [SerializeField] Vector2 iconDistances;
    private List<ItemAndObj> displayList = new List<ItemAndObj>();
    [SerializeField] Transform iconParent;
    RectTransform iconParentRect;
    [SerializeField] float scrollMultiplier = 0.5f;
    [SerializeField] List<Item> testItems = new();

    float scrollAmount = 0;

    void Start()
    {
        foreach (Item item in testItems)
        {
            AddItemToDisplay(item);
        }   
        iconParentRect = iconParent.GetComponent<RectTransform>();
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            UpdateScroll(scroll);
        }
    }

    public void UpdateScroll(float scroll)
    {   
        int rowCount = Mathf.CeilToInt(displayList.Count / (float) columns);
        float scrollDistance = (rowCount * iconDistances.y) - bottomDistance;
        if (scrollDistance <= 0) { scrollDistance = 0; }
        scrollAmount = Mathf.Clamp(scrollAmount - (scroll * scrollMultiplier), 0, scrollDistance);
        iconParentRect.anchoredPosition = new Vector2(0, scrollAmount);

        if (scrollDistance == 0)
        {
            slider.value = 0;
        }
        else
        {
            slider.value = scrollAmount / scrollDistance;
        }
    }

    public void AddItemToDisplay(Item item)
    {
        if (item is not Topping topping) { return; } // TODO make ingredients visible also
        GameObject newIcon = Instantiate(iconPrefab, iconParent);

        InventoryIconControl inventoryIconControl = newIcon.GetComponent<InventoryIconControl>();
        inventoryIconControl.assignedTopping = topping;
        newIcon.GetComponent<InventoryIconControl>().SetSprite(topping.shopSprite);

        ItemAndObj itemAndObj = new ItemAndObj(item, newIcon);
        displayList.Add(itemAndObj);

        UpdateAllIcons();
    }

    public void UpdateAllIcons()
    {
        int budgetEnum = 0;
        foreach (ItemAndObj itemAndObj in displayList)
        {
            itemAndObj.rect.anchoredPosition = iconStartPos +
                new Vector2(iconDistances.x * (budgetEnum % columns), -iconDistances.y * (budgetEnum / columns));
            budgetEnum += 1;
        }
    }

    private struct ItemAndObj
    {
        public ItemAndObj(Item item, GameObject obj)
        {
            this.item = item;
            this.obj = obj;
            this.rect = obj.GetComponent<RectTransform>();
        }

        public Item item;
        public GameObject obj;
        public RectTransform rect;
    }
}
