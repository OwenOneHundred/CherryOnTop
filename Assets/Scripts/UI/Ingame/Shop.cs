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
    [SerializeField] List<Item> currentItems = new();
    [SerializeField] Transform itemParent;

    void Start()
    {
        rect = GetComponent<RectTransform>();
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

    public void UpdateAllIcons()
    {
        for (int i = 0; i < currentItems.Count; i++) {
            GameObject newIcon = Instantiate(shopObjPrefab, itemParent);
            newIcon.GetComponent<ShopObj>().SetUp(currentItems[i]);
            newIcon.GetComponent<RectTransform>().anchoredPosition +=
                new Vector2((i % columns), (int) (-i / columns)) * iconSpacing;
        }
    }
}
