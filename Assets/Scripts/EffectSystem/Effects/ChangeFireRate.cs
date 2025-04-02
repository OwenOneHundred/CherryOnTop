using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeFireRate")]
public class ChangeFireRate : EffectSO
{
    bool firstTimeTriggered = true;
    float cooldown = 0;
    [SerializeField] float cooldownPercentChange = -0.075f;
    AttackManager attackManager;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (toppingObj == null) { return; }
        if (firstTimeTriggered)
        {
            attackManager = toppingObj.GetComponentInChildren<AttackManager>();
            cooldown = attackManager.GetAttackCooldown();
            firstTimeTriggered = false;
        }

        cooldown += cooldown * cooldownPercentChange;

        attackManager.SetAttackCooldown(cooldown);
    }
}
