using UnityEngine;
using System.Collections.Generic;

public class BuffManager : MonoBehaviour
{

public static BuffManager Instance;

    private Dictionary<AttackManager, List<BuffZone>> activeBuffs = 
        new Dictionary<AttackManager, List<BuffZone>>();

    void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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

        //tower.attack.damage = (tower.BaseCooldown * cooldownMultiplier);
        //tower.SetEffectiveDamage(Mathf.RoundToInt(tower.BaseDamage * damageMultiplier));
    }
}