using UnityEngine;
using EventBus;

[CreateAssetMenu(menuName = "Effects/Money/MoneyIfCherryFrozen")]
public class GetMoneyIfCherryDiedWasFrozen : EffectSO
{
    [SerializeField] int amountToChangeMoney = 1;
    [SerializeField] float chanceOfHappening0to1 = 1;
    [SerializeField] bool useRadius = true;
    TargetingSystem targetingSystem;
    public override void OnTriggered(IEvent eventObject)
    {
        if (targetingSystem == null) { targetingSystem = toppingObj.GetComponentInChildren<TargetingSystem>(); }
        if (Random.value > chanceOfHappening0to1) { return; }
        if (eventObject is CherryDiesEvent diesEvent)
        {
            Debug.Log(Vector3.Distance(toppingObj.transform.position, diesEvent.cherry.transform.position));
            if (useRadius && (Vector3.Distance(toppingObj.transform.position, diesEvent.cherry.transform.position) > targetingSystem.GetRange())) { return; }
            if (diesEvent.cherry.GetComponentInChildren<DebuffManager>().HasDebuffType(CherryDebuff.DebuffType.freeze))
            {
                Inventory.inventory.Money += amountToChangeMoney;
            }
        }
    }
}
