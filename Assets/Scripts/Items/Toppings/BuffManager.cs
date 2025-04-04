using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour {
    AttackManager attackManager;
    private TargetingSystem targetingSystem;

    private float baseCooldown;
    private int baseDamage;
    private float baseRange;

    private List<BuffZone> activeBuffs = new List<BuffZone>();
    
    void Start() {
        InitializeBuffManager();
    }

    void InitializeBuffManager() {
        attackManager = GetComponent<AttackManager>();
        targetingSystem = GetComponent<TargetingSystem>();

        baseDamage = attackManager.AttackDamage;
        baseCooldown = attackManager.GetAttackCooldown();

        baseRange = targetingSystem.GetRange();
    }

    public void AddBuff(BuffZone buffZone)
    {
        if (!activeBuffs.Contains(buffZone))
        {
            activeBuffs.Add(buffZone);
            RecalculateTowerStats();
        }
    }

    public void RemoveBuff(BuffZone buffZone)
    {
        if (activeBuffs.Contains(buffZone))
        {
            activeBuffs.Remove(buffZone);
            RecalculateTowerStats();
        }
    }

    void RecalculateTowerStats() {
        float cooldownMultiplier = 1f;
        float damageMultiplier = 1f;
        float rangeMultiplier = 1f;

        foreach (BuffZone buff in activeBuffs)
        {
            switch (buff.BuffType)
            {
                case BuffType.CooldownReduction:
                    cooldownMultiplier *= buff.BuffValue;
                    break;
                case BuffType.DamageBoost:
                    damageMultiplier *= buff.BuffValue;
                    break;
                case BuffType.RangeIncrease:
                    rangeMultiplier *= buff.BuffValue;
                    break;
            }
        }

        if (attackManager != null) {
            attackManager.SetAttackCooldown(baseCooldown * cooldownMultiplier);
            Debug.Log($"Tower {gameObject.name} Cooldown Update: {baseCooldown * cooldownMultiplier}");

            attackManager.AttackDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
            Debug.Log($"Tower {gameObject.name} Damage Update: {baseDamage * damageMultiplier}");
        }

        if (targetingSystem != null) {
            targetingSystem.SetRange(baseRange * rangeMultiplier);
            Debug.Log($"Tower {gameObject.name} Range Update: {targetingSystem.GetRange()}");
        }
    }
}
