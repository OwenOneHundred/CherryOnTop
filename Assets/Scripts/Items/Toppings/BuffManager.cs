using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class BuffManager : MonoBehaviour {
    //Creating all variables
    AttackManager attackManager;
    private TargetingSystem targetingSystem;
    private float baseCooldown;
    private int baseDamage;
    private float baseRange;
    private List<BuffZone> activeBuffs = new List<BuffZone>();
    
    void Start() {
        InitializeBuffManager();
    }

    /**
    * Initializes the BuffManager by getting references to the AttackManager and TargetingSystem components.
    * Also retrieves the base damage, cooldown, and range values from the AttackManager and TargetingSystem.
    */
    void InitializeBuffManager() {
        Debug.Log($"BuffManager initialized for {gameObject.name}");
        attackManager = GetComponentInChildren<AttackManager>();
        Debug.Log($" Found BuffManager component AttackManager: {attackManager != null}");
        targetingSystem = GetComponent<TargetingSystem>();

        if (attackManager != null)
        {
            Debug.Log("Base attacks set");
            baseDamage = attackManager.AttackDamage;
            baseCooldown = attackManager.GetAttackCooldown();
            Debug.Log($"Base Damage: {baseDamage}, Base Cooldown: {baseCooldown}");
        }

        if (targetingSystem != null)
        {
            baseRange = targetingSystem.GetRange();
        }
    }

    /**
    * Adds a buff to the tower if it is not already present in the activeBuffs list.
    * Calls RecalculateTowerStats to update the tower's stats based on the new buff.
    */
    public void AddBuff(BuffZone buffZone)
    {
        if (!activeBuffs.Contains(buffZone))
        {
            activeBuffs.Add(buffZone);
            RecalculateTowerStats();
        }
    }

    public void RemoveBuff(BuffZone buffZone, AttackManager attackManager)
    {
        Debug.Log("Removing buff");
        if (activeBuffs.Contains(buffZone))
        {
            activeBuffs.Remove(buffZone);
            RecalculateTowerStats();
        }
    }

    /**
    * Recalculates the tower's stats based on the active buffs.
    * Applies multipliers to cooldown, damage, and range based on the buffs present.
    * Updates the AttackManager and TargetingSystem with the new values.
    */
    // This function is called whenever a buff is added or removed.
    // It calculates the new cooldown, damage, and range based on the active buffs.
    // It uses the base values stored in the BuffManager to apply the multipliers.
    // The multipliers are applied in a way that allows for multiple buffs of the same type to stack.
    void RecalculateTowerStats() {
        Debug.Log($"Recalculating stats for {gameObject.name}");
        float cooldownMultiplier = 1f;
        float damageMultiplier = 1f;
        float rangeMultiplier = 1f;

        foreach (BuffZone buff in activeBuffs)
        {
            // Apply the buff's effect based on its type
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

        Debug.Log($"Buffs applied on {gameObject.name}: Cooldown {cooldownMultiplier}, Damage {damageMultiplier}, Range {rangeMultiplier}");
        // Update the AttackManager and TargetingSystem with the new values
        if (attackManager != null) {
            attackManager.SetAttackCooldown(baseCooldown * cooldownMultiplier);
            Debug.Log($"Tower {gameObject.name} Cooldown Update: {baseCooldown * cooldownMultiplier}");

            attackManager.AttackDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
            Debug.Log($"Tower {gameObject.name} Damage Update: {baseDamage * damageMultiplier}");
        } else {
            Debug.LogWarning($"BuffManager: AttackManager not found on {gameObject.name}");
        }
        if (targetingSystem != null) {
            targetingSystem.SetRange(baseRange * rangeMultiplier);
            Debug.Log($"Tower {gameObject.name} Range Update: {targetingSystem.GetRange()}");
        }
    }
}
