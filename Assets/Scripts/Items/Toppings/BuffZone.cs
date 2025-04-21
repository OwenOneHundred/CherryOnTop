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
    void OnTriggerEnter(Collider other) {
        Debug.Log("OnTriggerEnter called");

        // 1) Layer check
        if ((toppingLayer & (1 << other.gameObject.layer)) == 0)
            return;
        Debug.Log("Object is on topping layer");

        // 2) Find the ToppingObjectScript on parent
        var toppingObj = other.GetComponentInParent<ToppingObjectScript>();
        if (toppingObj == null)
        {
            Debug.Log("  → no ToppingObjectScript found");
            return;
        }
        Debug.Log("Object has ToppingObjectScript");

        // 3) Flag filter
        if ((toppingObj.topping.flags & flags) == 0)
        {
            Debug.Log("  → topping flags do not match");
            return;
        }
        Debug.Log("Object has the required topping flags");

        // 4) Find BuffManager on the same root
        var buffManager = other.GetComponentInParent<BuffManager>();
        if (buffManager == null)
        {
            Debug.Log("  → no BuffManager found");
            return;
        }
        Debug.Log("Found BuffManager");


        // 5) Finally apply buff if new
        if (!affectedToppings.Contains(buffManager))
        {
            Debug.Log("Calling AddBuff()");
            buffManager.AddBuff(this);
            affectedToppings.Add(buffManager);
        }
        else
        {
            Debug.Log("  → buff already applied to this tower");
        }
    }

    // When an object leaves the trigger zone:
    void OnTriggerExit(Collider other)
    {
        Debug.Log("OnTriggerExit called");
        if ((toppingLayer & (1 << other.gameObject.layer)) != 0)
        {
            if (other.TryGetComponent(out BuffManager buffManager))
            {
                if (affectedToppings.Contains(buffManager))
                {
                    other.TryGetComponent(out AttackManager attackManager);
                    buffManager.RemoveBuff(this,attackManager);
                    affectedToppings.Remove(buffManager);
                    Debug.Log($"BuffZone {gameObject.name} removed {buffType} buff from {other.gameObject.name}");
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
