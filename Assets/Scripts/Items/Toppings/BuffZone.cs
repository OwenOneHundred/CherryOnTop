using System.Collections.Generic;
using UnityEngine;

public class BuffZone : MonoBehaviour
{
    [SerializeField] private float buffRadius = 5f;
    [SerializeField] private ToppingTypes.Flags flag = ToppingTypes.Flags.sweet;
    [SerializeField] private BuffType buffType;
    [SerializeField] private float buffValue = 0.6f; // Buffs should be >1 for increase

    private LayerMask toppingLayer;
    private HashSet<BuffManager> affectedToppings = new HashSet<BuffManager>();

    public BuffType BuffType => buffType;
    public float BuffValue => buffValue;

    void Start()
    {
        toppingLayer = LayerMask.GetMask("Topping");
        UpdateBuffs();
    }

    void Update()
    {
        UpdateBuffs();
    }

    void UpdateBuffs()
    {
        Collider[] toppings = Physics.OverlapSphere(transform.position, buffRadius, toppingLayer);
        HashSet<BuffManager> currentTowers = new HashSet<BuffManager>();

        foreach (Collider col in toppings)
        {
            BuffManager buffManager = col.GetComponent<BuffManager>();
            if (buffManager != null && col != this)
            {
                currentTowers.Add(buffManager);
                if (!affectedToppings.Contains(buffManager))
                {
                    //Error line
                    buffManager.AddBuff(this);
                }
            }
        }

        foreach (BuffManager buffManager in new HashSet<BuffManager>(affectedToppings))
        {
            if (!currentTowers.Contains(buffManager) && buffManager != this)
            {
                buffManager.RemoveBuff(this);
                affectedToppings.Remove(buffManager);
            }
        }

        affectedToppings = currentTowers;
    }

    void OnDestroy()
    {
        foreach (BuffManager buffManager in affectedToppings)
        {
            buffManager.RemoveBuff(this);
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
