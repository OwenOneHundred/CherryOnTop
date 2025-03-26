using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{
    public static BuffManager Instance { get; private set; }

    private Dictionary<AttackManager, List<BuffZone>> activeBuffs = 
        new Dictionary<AttackManager, List<BuffZone>>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void AddBuff(AttackManager tower, BuffZone buffSource)
    {
        if (!activeBuffs.ContainsKey(tower))
        {
            activeBuffs[tower] = new List<BuffZone>();
        }

        if (!activeBuffs[tower].Contains(buffSource))
        {
            activeBuffs[tower].Add(buffSource);
            UpdateTowerStats(tower);
        }
    }

    public void RemoveBuff(AttackManager tower, BuffZone buffSource)
    {
        if (activeBuffs.ContainsKey(tower))
        {
            activeBuffs[tower].Remove(buffSource);
            UpdateTowerStats(tower);
        }
    }

    void UpdateTowerStats(AttackManager tower)
    {
        float cooldownMultiplier = 1f;
        float damageMultiplier = 1f;

        foreach (BuffZone buff in activeBuffs[tower])
        {
            switch (buff.BuffType)
            {
                case BuffType.CooldownReduction:
                    cooldownMultiplier *= buff.BuffValue;
                    break;
                case BuffType.DamageBoost:
                    damageMultiplier *= buff.BuffValue;
                    break;
            }
        }

        //tower.SetEffectiveCooldown(tower.BaseCooldown * cooldownMultiplier);
        //tower.SetEffectiveDamage(Mathf.RoundToInt(tower.BaseDamage * damageMultiplier));
    }
}