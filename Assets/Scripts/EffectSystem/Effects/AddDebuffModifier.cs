using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/AddDebuffModifier")]
public class AddDebuffModifier : EffectSO
{
    [SerializeField] DebuffModifier debuffModifier;
    public override void OnTriggered(IEvent eventObject)
    {
        DebuffModifierManager.Instance.AddDebuffModifier(debuffModifier);
    }
}
