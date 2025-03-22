using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/RemoveModifierForRemoveThis")]
public class RemoveModifierForSellThis : EffectSO
{
    [SerializeField] DebuffModifier debuffModifier;
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is SellEvent sellEvent)
        {
            if (sellEvent.toppingObj == toppingObj)
            {
                DebuffModifierManager.Instance.RemoveDebuffModifier(debuffModifier);
            }
        }
        else
        {
            Debug.LogWarning("Used RemoveModifiedForPlaceThis with an event that is not placeEvent");
        }
    }
}
