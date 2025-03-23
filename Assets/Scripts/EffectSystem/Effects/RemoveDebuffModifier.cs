using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/RemoveDebuffModifier")]
public class RemoveDebuffModifier : EffectSO
{
    [SerializeField] DebuffModifier debuffModifier;
    public override void OnTriggered(IEvent eventObject)
    {
        DebuffModifierManager.Instance.RemoveDebuffModifier(debuffModifier);
    }
}
