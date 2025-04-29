using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/ChangeMoneyChance")]
public class ChangeMoneyChance : EffectSO
{
    [SerializeField] int amountToChangeMoney = 1;
    [SerializeField] float chanceOfHappening0to1 = 1;
    public override void OnTriggered(IEvent eventObject)
    {
        if (Random.value <= chanceOfHappening0to1)
        {
            GetTopping().moneyGained += amountToChangeMoney;
            Inventory.inventory.Money += amountToChangeMoney;
            Debug.Log(toppingObj);
            GetToppingActivatedGlow().StartNewFireEffect("Orange", new Color(1, 0.57f, 0.2f), 2);
        }
    }
}
