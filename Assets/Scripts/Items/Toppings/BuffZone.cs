using System.Collections.Generic;
using UnityEngine;

public class BuffZone : MonoBehaviour {
    [SerializeField] private float buffRadius = 5f;
    [SerializeField] private ToppingTypes.Flags flag = ToppingTypes.Flags.sweet;
    [SerializeField] private BuffType buffType;
    [SerializeField] private float buffValue = 0.6f; // Buffs should be >1 for increase

    private LayerMask toppingLayer;
    private HashSet<BuffManager> affectedToppings = new HashSet<BuffManager>();

    public BuffType BuffType => buffType;
    public float BuffValue => buffValue; // the comment below doesn't apply to these, they just make a mini function that returns the lowercase one
    private Collider coll; // the => makes a lambda, so whatever you do after the => gets called whenever anyone accesses the variable.
    // That means "coll => GetComponent<Collider>();" says "whenever you need the collider, run GetComponent." This is much slower than
    // running GetComponent once and storing the value to be read directly whenever it's needed. I've changed the code to that way.

    void Start()
    {
        coll = GetComponent<Collider>();
        toppingLayer = LayerMask.GetMask("Topping");
    }

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
            if (foundCollider.TryGetComponent(out BuffManager buffManager) && foundCollider != coll) {

                currentTowers.Add(buffManager);
                
                if (!affectedToppings.Contains(buffManager) && foundCollider.TryGetComponent(out AttackManager _)){
                    buffManager.AddBuff(this);
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

    void OnDestroy() {
        foreach (BuffManager buffManager in affectedToppings)
        {
            buffManager.RemoveBuff(this);
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, buffRadius);
    }
}

//This enum is used to define the different types of buffs
//that can be applied to the towers.
public enum BuffType { 
    CooldownReduction,
    DamageBoost,
    RangeIncrease
}
