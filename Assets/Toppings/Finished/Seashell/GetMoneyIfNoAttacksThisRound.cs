using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/GetMoneyIfNoDamageThisRound")]
public class GetMoneyIfHitNothingThisRound : EffectSO
{
    [SerializeField] int money;
    public override void OnTriggered(IEvent eventObject)
    {
        if (toppingFirePointObj.transform.root.GetComponent<ToppingObjectScript>().topping.damagedCherriesThisRound <= 0)
        {
            Inventory.inventory.Money += money;
        }
    }
}
