using UnityEngine;

[CreateAssetMenu(menuName ="Effects/ChangeMoney")]
public class ChangeMoney : EffectSO
{
    [SerializeField] int amountToChangeMoney = 5;

    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        GetTopping().moneyGained += amountToChangeMoney;
        Inventory.inventory.Money += amountToChangeMoney;
        ToppingActivatedGlow toppingGlow = GetToppingActivatedGlow();
        if (toppingGlow != null) { toppingGlow.StartNewFireEffect("ChangeMoney", Color.yellow, 3); }
    }
}