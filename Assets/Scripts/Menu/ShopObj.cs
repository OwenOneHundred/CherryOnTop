using UnityEngine.UI;
using UnityEngine;

public abstract class ShopObj : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI priceText;
    public void SetUp(Item item)
    {
        image.sprite = item.shopSprite;
        nameText.text = item.name;
        priceText.text = item.price + "";
    }
}
