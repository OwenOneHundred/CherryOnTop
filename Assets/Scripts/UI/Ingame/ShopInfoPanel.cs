using UnityEngine;

public class ShopInfoPanel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI itemType;
    [SerializeField] TMPro.TextMeshProUGUI description;
    [SerializeField] TMPro.TextMeshProUGUI toppingType;
    Item item;

    void Start()
    {
        Clear();
    }

    public void SetUp(Item item)
    {
        this.item = item;
        if (item == null) { return; }
        if (item is Topping topping)
        {
            toppingType.text = InfoPopup.ToTitleCase(topping.flags.ToString());
            itemType.text = "Topping";
        }
        else
        {
            itemType.text = "Ingredient";
        }

        nameText.text = item.name.Replace("(Clone)", "");
        description.text = item.description;
    }

    public void Clear()
    {
        nameText.text = "";
        description.text = "";
        itemType.text = "";
        toppingType.text = "";
        item = null;
    }
}
