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

    [SerializeField] Color yesColor = new(255, 219, 231);
    [SerializeField] Color noColor = new(210, 10, 0);

    void Start()
    {
        GetComponent<SparkleSpawner>().SetUp(displayItem.rarity);
    }


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
            if (Inventory.inventory.TryBuyItem(displayItem))
            {
                FindFirstObjectByType<InventoryRenderer>().UpdateAllIconPositions();
                Shop.shop.UpdateAllIconText();
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
        GetComponent<Image>().sprite = item.rarity == ToppingTypes.Rarity.Common ? commonBG : (item.rarity == ToppingTypes.Rarity.Uncommon ? uncommonBG : rareBG);
        displayItem = item;
        UpdateInfo();
    }

    public IEnumerator IconAppearAnim(float delay)
    {
        float scaleSpeed = 20f;
        Vector3 goal = transform.localScale;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(delay);

        if (displayItem.rarity == ToppingTypes.Rarity.Rare)
        {
            SoundEffectManager.sfxmanager.PlayOneShot(Shop.shop.onRollRare);
        }

        if (this == null) { yield break; }
        while (transform.localScale != goal)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, goal, Time.deltaTime * scaleSpeed);
            yield return null;
            if (this == null) { yield break; }
        }
    }

    public void UpdateInfo()
    {
        image.sprite = displayItem.shopSprite;
        nameText.text = displayItem.name;
        priceText.text = "$" + displayItem.price;
        priceText.color = (Inventory.inventory.Money < displayItem.price) ? noColor : yesColor;
        priceText.outlineColor = (Inventory.inventory.Money < displayItem.price) ? noColor : yesColor;
    }
}
