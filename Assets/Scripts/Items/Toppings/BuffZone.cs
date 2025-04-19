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

/*
    void Update() {
        UpdateBuffs();
    }


    void UpdateBuffs() {
        Collider[] toppings = Physics.OverlapSphere(transform.position, buffRadius, toppingLayer);
        
        // Running the overlap sphere every frame is slower than using OnTriggerEnter and OnTriggerExit
        // and keeping a list of everything inside the collider.

        // Also, you're creating a hash set, creating a toppings array, moving everything from the array to the hash set, and then not using the array.
        // You could use the toppings array for everything "current towers" does (they're identical), and then add/remove from affectedToppings
        // so you don't have to delete and reallocate memory. 

        HashSet<BuffManager> currentTowers = new HashSet<BuffManager>(); // holds everything found this frame

        foreach (Collider foundCollider in toppings) {
            Debug.Log($"Found collider: {foundCollider.gameObject.name}");
            // using the line below for if check does work but probably costs more
             BuffManager buffManager = foundCollider.GetComponentInParent<BuffManager>();
            // foundCollider.TryGetComponent(out BuffManager buffManager);
            if (buffManager != null) {
                Debug.Log("found buff manager");
            } else {
                Debug.Log("found no buff manager");
            }       

    
            if (foundCollider != coll) {
                // This statement is not seen during tests
                Debug.Log($"Found BuffManager: {buffManager.gameObject.name} and collider is not self");

                currentTowers.Add(buffManager);
                
                if (!affectedToppings.Contains(buffManager) && foundCollider.TryGetComponent(out AttackManager attackManager)) {
                    // DID NOT PRINT BELOW
                    Debug.Log("NEW RECORD");
                    if(foundCollider.TryGetComponent(out ToppingObjectScript toppingObj)) {
                        if ((toppingObj.topping.flags & flags) != 0) {
                            // Check if the object's topping flags include the required flag.
                            // Get its BuffManager.
                            // Add the buff to the tower
                            Debug.Log($"BuffZone {gameObject.name} applied {buffType} buff to {foundCollider.gameObject.name}");
                            buffManager.AddBuff(this);
                        }
                    }
                }
            }
        }

        foreach (BuffManager buffManager in new HashSet<BuffManager>(affectedToppings)) {
            if (!currentTowers.Contains(buffManager) && buffManager != this) {
                buffManager.RemoveBuff(this);
            }
        }

        affectedToppings = currentTowers;
    }

*/

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


        // 6) Finally apply buff if new
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
