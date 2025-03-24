using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shop : MonoBehaviour
{
    public float speed = 10;
    bool open;
    bool moving;
    readonly float closedPos = -1690;
    readonly float openPos = -230;
    RectTransform rect;

    [SerializeField] int columns = 3;
    [SerializeField] int iconSpacing = 100;

    [SerializeField] GameObject shopObjPrefab;
    public List<Item> currentItems = new();
    public List<Item> availableItems = new();
    [SerializeField] Transform itemParent;
    public List<ShopObj> shopObjs = new();
    public int rerolls = 0;
    public int rerollPrice = 4;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        RerollItems();
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

            open = value;
            moving = true;

            StartCoroutine(Mover());
        }
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

        UpdateAllIcons();
    }

    public void OnClickReroll()
    {
        if (rerolls > 0)
        {
            rerolls -= 1;
            RerollItems();
        }
        else if (Inventory.inventory.Money > rerollPrice)
        {
            Debug.Log("rerollPrice: " + rerollPrice);
            Inventory.inventory.Money -= rerollPrice;
            RerollItems();
        }
        else 
        {
            // can't afford reroll
        }
    }

    public void OnRoundEnd()
    {
        RerollItems();
    }

    public void RerollItems()
    {
        foreach (ShopObj shopObj in shopObjs)
        {
            Destroy(shopObj.gameObject);
        }
        shopObjs.Clear();
        currentItems.Clear();
        PopulateShop();
        UpdateAllIcons();
    }

    public void PopulateShop()
    {
        // Will probably be changed later idk
        for (int i = 0; i < 6; i++)
        {
            int item = Random.Range(0, availableItems.Count);
            currentItems.Add(availableItems[item]);
        }
    }

    public void UpdateAllIcons()
    {
        for (int i = 0; i < currentItems.Count; i++) {
            GameObject newIcon = Instantiate(shopObjPrefab, itemParent);
            ShopObj shopObj = newIcon.GetComponent<ShopObj>();
            shopObjs.Add(shopObj);
            shopObj.SetUp(currentItems[i]);
            newIcon.GetComponent<RectTransform>().anchoredPosition +=
                new Vector2((i % columns), (int) (-i / columns)) * iconSpacing;
        }
    }

    public void UpdateAllIconText()
    {
        List<ShopObj> shopObjsCopy = new(shopObjs);
        foreach (ShopObj shopObj in shopObjsCopy)
        {
            // hack to manage shopobj list. would be better if shop icons were better tracked, removable, etc
            if (shopObj == null) { shopObjs.Remove(shopObj); continue; }
            
            shopObj.UpdateInfo();
        }
    }
}
