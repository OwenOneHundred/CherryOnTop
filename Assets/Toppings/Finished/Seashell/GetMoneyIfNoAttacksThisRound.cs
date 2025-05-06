using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyIfNoDamageThisRound")]
public class GetMoneyIfHitNothingThisRound : EffectSO
{
    [SerializeField] int money;
    public override void OnTriggered(IEvent eventObject)
    {
        if (toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping.damagedCherriesThisRound <= 0)
        {
            GetTopping().moneyGained += money;
            Inventory.inventory.Money += money;
        }
    }
}
