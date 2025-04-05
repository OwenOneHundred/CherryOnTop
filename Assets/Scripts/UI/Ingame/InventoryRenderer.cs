using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryRenderer : MonoBehaviour
{
    [SerializeField] GameObject iconPrefab;
    [SerializeField] int columns = 3;
    [SerializeField] int rows = 7;
    [SerializeField] Vector2 iconStartPos;
    [SerializeField] float bottomDistance = 488;
    [SerializeField] Vector2 iconDistances;
    private List<ItemAndObj> displayList = new List<ItemAndObj>();
    [SerializeField] Transform iconParent;
    RectTransform iconParentRect;

    int pages = 1;
    int currentPage = 1;
    int amountPerPage = 21;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;

    void Start()
    {
        iconParentRect = iconParent.GetComponent<RectTransform>();
        amountPerPage = columns * rows;
        UpdatePageCount();
    }

    public void AddItemToDisplay(Item item)
    {
        if (item is not Topping topping) { return; } // TODO make ingredients visible also

        bool itemExists = DisplayHasItem(item, out ItemAndObj existingItem);
        if (itemExists)
        {
            AddItemToStack(item, existingItem);
        }
        else
        {
            SpawnNewItem();
        }

        UpdatePageCount();
        UpdateAllIconPositions();

        void SpawnNewItem()
        {
            GameObject newIcon = Instantiate(iconPrefab, iconParent);

            InventoryIconControl inventoryIconControl = newIcon.GetComponent<InventoryIconControl>();
            inventoryIconControl.assignedTopping = topping;
            newIcon.GetComponent<InventoryIconControl>().SetUp(topping.shopSprite);

            ItemAndObj itemAndObj = new ItemAndObj(item, newIcon);
            displayList.Add(itemAndObj);
        }

        void AddItemToStack(Item itemToAdd, ItemAndObj iconToAddTo)
        {
            iconToAddTo.obj.GetComponent<InventoryIconControl>().AmountInStack += 1;
        }
    }

    private bool DisplayHasItem(Item item, out ItemAndObj itemAndObj)
    {
        foreach (ItemAndObj existingIcon in displayList)
        {
            itemAndObj = existingIcon;
            if (existingIcon.item.name == item.name)
            {
                return true;
            }
        }
        itemAndObj = default;
        return false;
    }

    public int RemoveOneFromItemFromDisplay(Item item)
    {
        if (item is not Topping topping) { return 0; } // Remove this once there's functionality for ingredients in display

        ItemAndObj itemAndObj = displayList.First(x => x.item == topping);
        InventoryIconControl iconControl = itemAndObj.obj.GetComponent<InventoryIconControl>();

        iconControl.AmountInStack -= 1;
        if (iconControl.AmountInStack <= 0)
        {
            Destroy(itemAndObj.obj);
            displayList.Remove(itemAndObj);
        }

        UpdatePageCount();
        UpdateAllIconPositions();

        return iconControl.AmountInStack;
    }

    public void UpdateAllIconPositions()
    {
        foreach (ItemAndObj itemAndObj in displayList)
        {
            itemAndObj.obj.SetActive(false);
        }

        List<ItemAndObj> itemsOnPage = new();
        int start = amountPerPage * (currentPage - 1);
        int totalItems = displayList.Count;
        for (int i = start; i < start + amountPerPage; i++)
        {
            if (totalItems <= i) { break; }

            itemsOnPage.Add(displayList[i]);
        }

        int budgetEnum = 0;
        foreach (ItemAndObj itemAndObj in itemsOnPage)
        {
            itemAndObj.obj.SetActive(true);
            itemAndObj.rect.anchoredPosition = iconStartPos +
                new Vector2(iconDistances.x * (budgetEnum % columns), -iconDistances.y * (budgetEnum / columns));
            budgetEnum += 1;
        }
    }

    public void NextPage()
    {
        currentPage += 1;
        if (currentPage > pages)
        {
            currentPage = 1;
        }

        UpdateAllIconPositions();
        UpdateArrows();
    }

    public void PreviousPage()
    {
        currentPage -= 1;
        if (currentPage < 0)
        {
            currentPage = pages;
        }

        UpdateAllIconPositions();
        UpdateArrows();
    }
    
    private void UpdatePageCount()
    {
        pages = (Inventory.inventory.ownedItems.Count() / amountPerPage) + 1;
        if (currentPage > pages)
        {
            currentPage = pages;
        }

        UpdateArrows();
    }

    private void UpdateArrows()
    {
        if (currentPage == pages)
        {
            rightArrow.SetActive(false);
        }
        else { rightArrow.SetActive(true); }

        if (currentPage == 1)
        {
            leftArrow.SetActive(false);
        }
        else { leftArrow.SetActive(true); }
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
