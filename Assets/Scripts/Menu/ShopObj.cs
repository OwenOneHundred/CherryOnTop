using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public abstract class ShopObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool hovered = false;
    bool purchased = false;
    Item displayItem;

    [SerializeField] Image image;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI priceText;

    [SerializeField] float selectedSize = 1.5f;
    [SerializeField] float deselectedSize = 1.25f;

    [SerializeField] Sprite commonBG;
    [SerializeField] Sprite uncommonBG;
    [SerializeField] Sprite rareBG;


    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
        Shop.shop.shopInfoPanel.SetUp(displayItem);
        StartCoroutine(SelectAnim());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        Shop.shop.shopInfoPanel.Clear();
        StartCoroutine(DeselectAnim());
    }

    public IEnumerator SelectAnim()
    {
        float speed = 5f;
        Vector3 goal = new Vector3(selectedSize, selectedSize, selectedSize);
        while (image.transform.localScale != goal && hovered && !purchased)
        {
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, goal, Time.deltaTime * speed);
            yield return null;
        }
    }

    public IEnumerator DeselectAnim()
    {
        float speed = 5f;
        Vector3 goal = new Vector3(deselectedSize, deselectedSize, deselectedSize);
        while (image.transform.localScale != goal && (!hovered || purchased))
        {
            image.transform.localScale = Vector3.Lerp(image.transform.localScale, goal, Time.deltaTime * speed);
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && hovered && !purchased)
        {
            Inventory inv = FindFirstObjectByType<Inventory>();
            if (inv.TryBuyItem(displayItem))
            {
                FindFirstObjectByType<InventoryRenderer>().UpdateAllIconPositions();
                image.color = Color.gray;
                nameText.text = "Purchased";
                priceText.enabled = false;
                purchased = true;
                StartCoroutine(DeselectAnim());
            }

        }
    }

    public void SetUp(Item item)
    {
        image.sprite = item.shopSprite;
        nameText.text = item.name;
        priceText.text = "$" + item.price;
        GetComponent<Image>().sprite = item.rarity == ToppingTypes.Rarity.Common ? commonBG : (item.rarity == ToppingTypes.Rarity.Uncommon ? uncommonBG : rareBG);
        displayItem = item;
    }

    public IEnumerator IconAppearAnim(float delay)
    {
        float scaleSpeed = 10f;
        Vector3 goal = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(delay);

        while (transform.localScale != goal)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, goal, Time.deltaTime * scaleSpeed);
            yield return null;
        }
    }

    public void UpdateInfo()
    {
        image.sprite = displayItem.shopSprite;
        nameText.text = displayItem.name;
        priceText.text = displayItem.price + "";
    }
}
