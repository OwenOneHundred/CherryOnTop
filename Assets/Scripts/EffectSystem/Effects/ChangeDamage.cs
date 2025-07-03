using System;
using GameSaves;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Effects/ChangeDamage")]
public class ChangeDamage : EffectSO
{
    bool initializeOnCall = true;
    int damage = 0;
    [SerializeField] int damageChange = -2;
    [SerializeField] bool sellWhenReached0 = false;
    AttackManager attackManager;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (toppingObj == null) { return; }
        if (initializeOnCall)
        {
            attackManager = toppingObj.GetComponentInChildren<AttackManager>();
            damage = attackManager.AttackDamage;
            initializeOnCall = false;
        }

        damage = Mathf.Clamp(damage + damageChange, 0, int.MaxValue);
        attackManager.AttackDamage = damage;

        if (sellWhenReached0 && damage <= 0)
        {
            Shop.shop.SellItemOffCake(GetTopping(), toppingObj);
        }
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
