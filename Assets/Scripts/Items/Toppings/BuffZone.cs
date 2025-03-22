using System.Collections.Generic;
using UnityEngine;

public class BuffZone : MonoBehaviour
{
    [SerializeField] private float buffRadius = 5f;
    [SerializeField] private string targetTag = "Sweet"; // Affects only towers with this tag
    [SerializeField] private BuffType buffType;
    [SerializeField] private float buffValue = 0.67f; // Default: cooldown multiplier (1.5x fire rate = 0.67 cooldown)

    private LayerMask towerLayer;
    private HashSet<AttackManager> buffedTowers = new HashSet<AttackManager>();

    void Start()
    {
        towerLayer = LayerMask.GetMask("Tower"); 
        ApplyBuffToNearbyTowers();
    }

    void Update()
    {
        CheckForNewTowers();
    }

    void ApplyBuffToNearbyTowers()
    {
        Collider[] towersInRange = Physics.OverlapSphere(transform.position, buffRadius, towerLayer);

        foreach (Collider col in towersInRange)
        {
            AttackManager attackManager = col.GetComponent<AttackManager>();
            if (attackManager != null && col.CompareTag(targetTag) && !buffedTowers.Contains(attackManager))
            {
                ApplyBuff(attackManager);
                buffedTowers.Add(attackManager);
            }
        }
    }

    void CheckForNewTowers()
    {
        Collider[] towersInRange = Physics.OverlapSphere(transform.position, buffRadius, towerLayer);

        foreach (Collider col in towersInRange)
        {
            AttackManager attackManager = col.GetComponent<AttackManager>();
            if (attackManager != null && col.CompareTag(targetTag) && !buffedTowers.Contains(attackManager))
            {
                ApplyBuff(attackManager);
                buffedTowers.Add(attackManager);
            }
        }

        // Remove buff if a tower moves out of range
        buffedTowers.RemoveWhere(tower =>
        {
            if (tower == null || Vector3.Distance(transform.position, tower.transform.position) > buffRadius)
            {
                RemoveBuff(tower);
                return true;
            }
            return false;
        });
    }

    void ApplyBuff(AttackManager attackManager)
    {
        if (buffType == BuffType.CooldownReduction)
        {
            attackManager.SetAttackCooldown(buffValue);
        }
        // Extend with more buffs as needed
    }

    void RemoveBuff(AttackManager attackManager)
    {
        if (buffType == BuffType.CooldownReduction)
        {
            attackManager.SetAttackCooldown(originalCooldown);
        }
        // Extend with more buffs as needed
    }

    private void OnDestroy()
    {
        // Remove buff when tower is destroyed
        foreach (AttackManager tower in buffedTowers)
        {
            RemoveBuff(tower);
        }
        buffedTowers.Clear();
    }
}

public enum BuffType
{
    CooldownReduction,
    DamageBoost,
    RangeIncrease
}
