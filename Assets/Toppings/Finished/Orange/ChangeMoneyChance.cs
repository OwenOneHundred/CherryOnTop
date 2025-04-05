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
        }
    }
}
