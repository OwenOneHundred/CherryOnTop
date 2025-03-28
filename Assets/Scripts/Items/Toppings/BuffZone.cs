using System.Collections.Generic;
using UnityEngine;

public class BuffZone : MonoBehaviour {
    [SerializeField] private float buffRadius = 5f;
    [SerializeField] private string targetTag = "Sweet";
    [SerializeField] private BuffType buffType;
    [SerializeField] private float buffValue = 0.67f;

    private LayerMask towerLayer;
    private HashSet<AttackManager> affectedTowers = new HashSet<AttackManager>();

    public BuffType BuffType => buffType;
    public float BuffValue => buffValue;

    void Start()
    {
        towerLayer = LayerMask.GetMask("Tower");
        UpdateBuffs();
    }

    void Update()
    {
        // Continuously check and apply buffs to nearby towers
        UpdateBuffs();
    }

    void UpdateBuffs()
    {
        
         if (BuffManager.Instance == null)
        {
            Debug.LogWarning("BuffManager instance not found!");
            return;
        }
        

        Collider[] towers = Physics.OverlapSphere(transform.position, buffRadius, towerLayer);
        HashSet<AttackManager> currentTowers = new HashSet<AttackManager>();

        foreach (Collider col in towers)
        {
            if (col.CompareTag(targetTag))
            {
                AttackManager am = col.GetComponent<AttackManager>();
                if (am != null)
                {
                    currentTowers.Add(am);
                    if (!affectedTowers.Contains(am))
                    {
                        BuffManager.Instance.AddBuff(am, this);
                    }
                }
            }
        }

        // Remove towers that left the range
        foreach (AttackManager am in new HashSet<AttackManager>(affectedTowers))
        {
            if (!currentTowers.Contains(am))
            {
                BuffManager.Instance.RemoveBuff(am, this);
                affectedTowers.Remove(am);
            }
        }

        affectedTowers = currentTowers;
    }
    

    void OnDestroy()
    {
        foreach (AttackManager am in affectedTowers)
        {
            BuffManager.Instance.RemoveBuff(am, this);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, buffRadius);
    }
}

public enum BuffType
{
    CooldownReduction,
    DamageBoost,
    RangeIncrease
}
