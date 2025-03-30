using System;
using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeDamageIfBoughtSweet")]
public class ChangeDamageIfBoughtSweet : EffectSO
{
    bool firstTimeTriggered = true;
    int damage = 0;
    [SerializeField] int damageChange = -2;
    AttackManager attackManager;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (toppingObj == null) { return; }

        if (!GetBoughtSweetTopping(eventObject))
        {
            return;
        }

        if (firstTimeTriggered)
        {
            attackManager = toppingObj.GetComponentInChildren<AttackManager>();
            damage = attackManager.AttackDamage;
            firstTimeTriggered = false;
        }

        damage = Mathf.Clamp(damage + damageChange, 0, int.MaxValue);
        attackManager.AttackDamage = damage;
    }

    bool GetBoughtSweetTopping(IEvent eventObject)
    {
        return eventObject is BuyEvent buyEvent && buyEvent.item is Topping topping && topping.flags.HasFlag(ToppingTypes.Flags.sweet);
    }
}
