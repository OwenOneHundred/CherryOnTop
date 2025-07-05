using System.Collections.Generic;
using UnityEngine;

public class BuffZone : MonoBehaviour {
    [SerializeField] private ToppingTypes.Flags flags;
    [SerializeField] private BuffType buffType;
    [SerializeField] private float buffValue = 0.6f; // Buffs should be >1 for increase

    private LayerMask toppingLayer;
    private HashSet<BuffManager> affectedToppings = new HashSet<BuffManager>();

    public BuffType BuffType => buffType;
    public float BuffValue => buffValue; // the comment below doesn't apply to these, they just make a mini function that returns the lowercase one

    void Awake()
    {
        toppingLayer = LayerMask.GetMask("Topping");
        GetComponentInChildren<SphereCollider>().radius = GetComponentInChildren<TargetingSystem>().GetRange();
    }

    // When an object enters the trigger zone:
    void OnTriggerEnter(Collider other)
    {
        // 1) Layer check
        if ((toppingLayer & (1 << other.gameObject.layer)) == 0)
            return;

        // 2) Find the ToppingObjectScript on parent
        var toppingObj = other.GetComponentInParent<ToppingObjectScript>();
        if (toppingObj == null)
        {
            return;
        }

        // 3) Flag filter
        if ((toppingObj.topping.flags & flags) == 0)
        {
            return;
        }

        // 4) Find BuffManager on the same root
        var buffManager = other.GetComponentInParent<BuffManager>();
        if (buffManager == null)
        {
            return;
        }

        // 5) Finally apply buff if new
        if (!affectedToppings.Contains(buffManager))
        {
            buffManager.AddBuff(this);
            affectedToppings.Add(buffManager);
        }
    }

    // When an object leaves the trigger zone:
    void OnTriggerExit(Collider other)
    {
        if ((toppingLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out BuffManager buffManager))
            {
                if (affectedToppings.Contains(buffManager))
                {
                    other.TryGetComponent(out AttackManager attackManager);
                    buffManager.RemoveBuff(this,attackManager);
                    affectedToppings.Remove(buffManager);
                }
            }
        }
    }
    

    void OnDestroy() {
        foreach (BuffManager buffManager in affectedToppings)
        {
            if (buffManager == null) { continue; }
            buffManager.RemoveBuff(this, buffManager.GetComponentInParent<AttackManager>());
        }
    }
}

//This enum is used to define the different types of buffs
//that can be applied to the towers.
public enum BuffType { 
    CooldownReduction,
    DamageBoost,
    RangeIncrease
}
