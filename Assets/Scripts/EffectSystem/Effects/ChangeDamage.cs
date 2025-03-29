using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Effects/ChangeDamageEveryRound")]
public class ChangeDamage : EffectSO
{
    bool firstTimeTriggered = true;
    int damage = 0;
    [SerializeField] int damageChange = -2;
    AttackManager attackManager;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (toppingObj == null) { return; }
        if (firstTimeTriggered)
        {
            attackManager = toppingObj.GetComponentInChildren<AttackManager>();
            damage = attackManager.AttackDamage;
            firstTimeTriggered = false;
        }

        damage = Mathf.Clamp(damage + damageChange, 0, int.MaxValue);
        attackManager.AttackDamage = damage;
    }
}
