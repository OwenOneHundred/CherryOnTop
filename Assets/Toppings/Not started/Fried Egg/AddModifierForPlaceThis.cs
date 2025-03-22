using UnityEngine;
using EventBus;

[CreateAssetMenu(menuName = "Effects/AddModifierForPlaceThis")]
public class AddModifierForPlaceThis : EffectSO
{
    [SerializeField] DebuffModifier debuffModifier;
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is TowerPlacedEvent placeTower)
        {
            if (placeTower.newToppingObj == toppingObj)
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