using System.Collections.Generic;
using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/DebuffNearbyToppingsOnTypePlaced")]
public class DebuffNearbyCherriesOnTypePlaced : EffectSO
{
    [SerializeField] CherryDebuff freeze;
    [SerializeField] ToppingTypes.Flags toppingTypesThatCount;
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is TowerPlacedEvent towerPlacedEvent &&
            towerPlacedEvent.topping != null &&
            towerPlacedEvent.topping.flags.HasAny(toppingTypesThatCount))
        {
            var cherries = toppingObj.GetComponentInChildren<TargetingSystem>().GetCherriesInRange();
            foreach (Collider cherry in cherries)
            {
                cherry.transform.root.GetComponent<DebuffManager>().AddDebuff(freeze);
            }
        }
    }
}
