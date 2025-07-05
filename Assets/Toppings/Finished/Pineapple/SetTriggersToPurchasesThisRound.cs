using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Special/SetTriggersToPurchasesThisRound")]
public class SetTriggersToPurchasesThisRound : EffectSO
{
    public override void OnTriggered(IEvent eventObject)
    {
        GetTopping().TriggersCount = Shop.shop.purchasesThisRound;
    }
}
