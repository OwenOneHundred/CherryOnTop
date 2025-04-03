using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeRange")]
public class IncreaseRange : EffectSO
{
    [SerializeField] float percentageToChangeRange = 0.1f;
    bool firstCall = true;
    TargetingSystem targetingSystem;

    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (firstCall)
        {
            targetingSystem = toppingObj.GetComponentInChildren<TargetingSystem>();
            firstCall = false;
        }
        targetingSystem.SetRange(targetingSystem.GetRange() * (1 + percentageToChangeRange));
        ToppingActivatedGlow toppingGlow = GetToppingActivatedGlow();
        if (toppingGlow != null) {
            toppingGlow.StartNewFireEffect("ChangeRange", Color.yellow, 3);
        }
    }
}
