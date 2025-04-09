using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyIfPlacedInRange")]
public class Get1IfPlacedInRange : EffectSO
{
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is TowerPlacedEvent towerPlacedEvent)
        {
            if (Vector3.Distance(toppingObj.transform.position, towerPlacedEvent.newToppingObj.transform.position) <= toppingObj.GetComponentInChildren<TargetingSystem>().GetRange())
            {
                Inventory.inventory.Money += 1;
                ToppingActivatedGlow toppingActivatedGlow = GetToppingActivatedGlow();
                if (toppingActivatedGlow != null)
                {
                    toppingActivatedGlow.StartNewFireEffect("Red", Color.red, 2);
                }
            }
        }
    }
}
