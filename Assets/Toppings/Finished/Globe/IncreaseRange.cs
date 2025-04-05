using EventBus;
using GameSaves;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeRange")]
public class IncreaseRange : EffectSO
{
    [SerializeField] float percentageToChangeRange = 0.1f;
    float range = 0;
    bool firstCall = true;
    TargetingSystem targetingSystem;

    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (firstCall)
        {
            targetingSystem = toppingObj.GetComponent<TargetingSystem>();
            firstCall = false;
        }
        targetingSystem.SetRange(range * (1 + percentageToChangeRange));
        ToppingActivatedGlow toppingGlow = GetToppingActivatedGlow();
        if (toppingGlow != null) {
            toppingGlow.StartNewFireEffect("ChangeRange", Color.yellow, 3);
        }
    }

    public override void Save(SaveData saveData)
    {
        DEFloatEntry floatEntry = new DEFloatEntry(GetID() + "-Range", this.range);
        saveData.SetDataEntry(floatEntry, true);
    }

    public override void Load(SaveData saveData)
    {
        targetingSystem = toppingObj.GetComponent<TargetingSystem>();
        firstCall = false;
        if (saveData.TryGetDataEntry(GetID() + "-Range", out DEFloatEntry floatEntry))
        {
            this.range = floatEntry.value;
            targetingSystem.SetRange(range);
        } 
        else 
        {
            this.range = targetingSystem.GetRange();
        }
    }
}
