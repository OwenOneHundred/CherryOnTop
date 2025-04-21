using System;
using GameSaves;
using UnityEngine;

[CreateAssetMenu(menuName = "Effects/ChangeFireRate")]
public class ChangeFireRate : EffectSO
{
    bool initializeOnCall = true;
    float cooldown = 0;
    [SerializeField] float cooldownPercentChange = -0.075f;
    AttackManager attackManager;
    public override void OnTriggered(EventBus.IEvent eventObject)
    {
        if (toppingObj == null) { return; }
        if (initializeOnCall)
        {
            attackManager = toppingObj.GetComponentInChildren<AttackManager>();
            cooldown = attackManager.GetAttackCooldown();
            initializeOnCall = false;
        }

        cooldown += cooldown * cooldownPercentChange;
        cooldown = Mathf.Clamp(cooldown, 0.1f, 100000);

        attackManager.SetAttackCooldown(cooldown);
    }

    public override void Save(SaveData saveData)
    {
        DEFloatEntry floatEntry = new DEFloatEntry(GetID() + "-Cooldown", this.cooldown);
        saveData.SetDataEntry(floatEntry, true);
    }

    public override void Load(SaveData saveData)
    {
        attackManager = toppingObj.GetComponent<AttackManager>();
        initializeOnCall = false;
        if (saveData.TryGetDataEntry(GetID() + "-Cooldown", out DEFloatEntry floatEntry))
        {
            this.cooldown = floatEntry.value;
            attackManager.SetAttackCooldown(cooldown);
        } 
        else 
        {
            this.cooldown = attackManager.AttackDamage;
        }
    }
}
