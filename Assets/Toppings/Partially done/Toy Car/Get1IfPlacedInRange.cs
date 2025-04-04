using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyIfPlacedInRange")]
public class Get1IfPlacedInRange : EffectSO
{
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is TowerPlacedEvent towerPlacedEvent)
        {
            if (Vector3.Distance(toppingFirePointObj.transform.position, towerPlacedEvent.newToppingObj.transform.position) <= toppingFirePointObj.GetComponent<TargetingSystem>().GetRange())
            {
                Inventory.inventory.Money += 1;
            }
        }
    }
}
