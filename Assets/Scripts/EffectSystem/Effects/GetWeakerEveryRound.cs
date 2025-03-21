using System;
using UnityEngine;

public class GetWeakerEveryRound : EffectSO
{
    [SerializeField] int damage = 10;
    [SerializeField] int damageChangePerRound = -2;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        damage = Mathf.Clamp(damage + damageChangePerRound, 0, int.MaxValue);
        toppingObj.GetComponent<AttackManager>().AttackDamage = damage;
    }
}
