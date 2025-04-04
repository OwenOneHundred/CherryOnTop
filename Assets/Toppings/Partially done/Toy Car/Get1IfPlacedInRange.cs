using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyIfPlacedInRange")]
public class Get1IfPlacedInRange : EffectSO
{
    public override void OnTriggered(IEvent eventObject)
    {
        if (eventObject is TowerPlacedEvent towerPlacedEvent)
        {
            Debug.Log(toppingObj.name);
            Debug.Log(toppingObj.GetComponent<TargetingSystem>());
            Debug.Log(towerPlacedEvent.newToppingObj);
            Debug.Log(toppingObj);

            if (Vector3.Distance(toppingObj.transform.position, towerPlacedEvent.newToppingObj.transform.position) <= toppingObj.GetComponent<TargetingSystem>().GetRange())
            {
                Inventory.inventory.Money += 1;
            }
        }
    }
}
