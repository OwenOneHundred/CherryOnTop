using System;
using EventBus;
using GameSaves;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeDamageIfBoughtSweet")]
public class ChangeDamageIfBoughtSweet : EffectSO
{
    bool initializeOnCall = true;
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

        if (initializeOnCall)
        {
            attackManager = toppingObj.GetComponentInChildren<AttackManager>();
            damage = attackManager.AttackDamage;
            initializeOnCall = false;
        }

        damage = Mathf.Clamp(damage + damageChange, 0, int.MaxValue);
        attackManager.AttackDamage = damage;
    }

    bool GetBoughtSweetTopping(IEvent eventObject)
    {
        return eventObject is BuyEvent buyEvent && buyEvent.item is Topping topping && topping.flags.HasFlag(ToppingTypes.Flags.sweet);
    }

    public override void Save(SaveData saveData)
    {
        DEIntEntry intEntry = new DEIntEntry(GetID() + "-Damage", this.damage);
        saveData.SetDataEntry(intEntry, true);
    }

    public override void Load(SaveData saveData)
    {
        attackManager = toppingObj.GetComponentInChildren<AttackManager>();
        initializeOnCall = false;
        if (saveData.TryGetDataEntry(GetID() + "-Damage", out DEIntEntry intEntry))
        {
            this.damage = intEntry.value;
            attackManager.AttackDamage = damage;
        } 
        else 
        {
            this.damage = attackManager.AttackDamage;
        }
    }
}
