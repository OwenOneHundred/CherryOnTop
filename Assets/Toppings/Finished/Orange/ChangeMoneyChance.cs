using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeMoneyChance")]
public class ChangeMoneyChance : EffectSO
{
    [SerializeField] int amountToChangeMoney = 1;
    [SerializeField] float chanceOfHappening0to1 = 1;
    public override void OnTriggered(IEvent eventObject)
    {
        if (Random.value <= chanceOfHappening0to1)
        {
            Inventory.inventory.Money += amountToChangeMoney;
            GetToppingActivatedGlow().StartNewFireEffect("Orange", new Color(1, 0.57f, 0.2f), 2);
        }
    }
}
