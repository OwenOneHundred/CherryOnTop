using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/Money/GetMoneyIfPlacedInRange")]
public class Get1IfPlacedInRange : EffectSO
{
    uint roundNumberOnLastTrigger = 0;
    int triggersThisRound = 0;
    [SerializeField] int maxTriggers = 5;
    public override void OnTriggered(IEvent eventObject)
    {
        if (RoundManager.roundManager.roundNumber == roundNumberOnLastTrigger)
        {
            triggersThisRound += 1;
        }
        else
        {
            roundNumberOnLastTrigger = RoundManager.roundManager.roundNumber;
            triggersThisRound = 0;
        }

        if (triggersThisRound > maxTriggers) { return; }
        
        if (eventObject is TowerPlacedEvent towerPlacedEvent)
        {
            if (Vector3.Distance(toppingObj.transform.position, towerPlacedEvent.newToppingObj.transform.position) <= toppingObj.GetComponentInChildren<TargetingSystem>().GetRange())
            {
                Inventory.inventory.Money += 1;
                PlayTriggeredSound();
                ToppingActivatedGlow toppingActivatedGlow = GetToppingActivatedGlow();
                if (toppingActivatedGlow != null)
                {
                    toppingActivatedGlow.StartNewFireEffect("Red", Color.red, 2);
                }
            }
        }
    }
}
