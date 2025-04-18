using UnityEngine;

public class ShopInfoPanel : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI description;
    [SerializeField] TMPro.TextMeshProUGUI toppingType;
    [SerializeField] TMPro.TextMeshProUGUI detailedInfo;
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
            SetUpDetailedStats(topping);
        }

        nameText.text = item.name;
        description.text = item.description;
    }

    private void SetUpDetailedStats(Topping topping)
    {
        AttackManager attackManager = topping.towerPrefab.GetComponentInChildren<AttackManager>();
        TargetingSystem targetingSystem = topping.towerPrefab.GetComponentInChildren<TargetingSystem>();
        string range = "-";
        string cooldown = "-";
        string damage = "-";
        if (attackManager != null)
        {
            cooldown = attackManager.attackTemplate.cooldown + "";
            damage = attackManager.attackTemplate.damage + "";
        }
        if (targetingSystem != null)
        {
            range = targetingSystem.GetRange() + "";
        }
        detailedInfo.text = "Damage: " + damage + "  |  Cooldown: " + cooldown + "  |  Range: " + range + "  |  Rarity: " + topping.rarity.ToString();
    }

    public void Clear()
    {
        nameText.text = "";
        description.text = "";
        toppingType.text = "";
        detailedInfo.text = "";
        item = null;
    }
}
