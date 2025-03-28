using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeDamageEveryRound")]
public class GetWeakerEveryRound : EffectSO
{
    bool firstTimeTriggered = true;
    int damage = 0;
    [SerializeField] int damageChangePerRound = -2;
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

        damage = Mathf.Clamp(damage + damageChangePerRound, 0, int.MaxValue);
        attackManager.AttackDamage = damage;
    }
}
