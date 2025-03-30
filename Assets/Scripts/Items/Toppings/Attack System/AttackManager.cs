using UnityEngine;

/// <summary>
/// Script that handles the attack cycle for a Topping. When a Topping is initialized, it must be given an attack type
/// using the SetAttack method.
/// </summary>
public class AttackManager : MonoBehaviour
{
    // Represents the type of attack to use, which holds information about the Topping's attack behaviour
    [SerializeField]
    ToppingAttack attackTemplate;

    // Stores a copy of this attack for this particular Topping
    private ToppingAttack attack;

    [SerializeField]
    // Keeps track of the current targeted Cherry. Should be null if there are no Cherries in range
    GameObject targetedCherry;

    // Represents whether the Topping is stunned or not. When stunned, Toppings cannot attack and their cooldown resets.
    private bool isStunned;

    // Keeps track of when the last attack was to time the next one
    private float timer = 0;

    // Keeps track of how many seconds are left before this Topping is no longer stunned.
    private float stunTimer = 0;

    public int AttackDamage
    {
        get { return attack.damage; }
        set
        {
            attack.damage = value;
        }
    }

    void Start()
    {
        this.attack = Instantiate(attackTemplate);
        this.attack.toppingObj = gameObject;
        if (this.attack != null) {
            this.attack.OnStart();
        }

        timer = attack.cooldown;
    }

    void Update()
    {
        if (!isStunned) {
            if (targetedCherry != null) {
                if (timer >= attack.cooldown) {
                    timer = 0;
                    attack.OnCycle(this.targetedCherry);
                }
            }
            if (timer <= attack.cooldown) {
                timer += Time.deltaTime;
            }
        } else {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) {
                stunTimer = 0;
                isStunned = false;
            }
        }
    }

    /// <summary>
    /// Switches targets to a new Cherry or to null if appropriate.
    /// </summary>
    /// <param name="targetedCherry"></param> 
    public void UpdateTargetedCherry(GameObject targetedCherry) {
        if (this.targetedCherry != targetedCherry) {
            this.targetedCherry = targetedCherry;
            if (this.targetedCherry != null) {
                attack.OnNewCherryFound(this.targetedCherry);
            }
        }       
    }

    /// <summary>
    /// Stun this Topping for a specified number of seconds.
    /// </summary>
    /// <param name="duration"></param>
    public void Stun(float duration) {
        this.isStunned = true;
        this.stunTimer = duration;
    }

    /// <summary>
    /// Sets this Topping's attack type to a specified ToppingAttack.
    /// </summary>
    /// <param name="attack"></param>
    public void SetAttack(ToppingAttack attack) {
        if (this.attack != attack) {
            this.attack = attack;
            this.attack.OnStart();
        }
    }

    /// <summary> 
    /// Sets this Topping's attack cooldown to a specified value. If the new value is different to the previous
    /// value, the timer is reset (the Topping waits the full length of the cooldown before attacking again).
    /// </summary>
    /// <param name="cooldown"></param>
    public void SetAttackCooldown(float cooldown) {
        if (cooldown != this.attack.cooldown) {
            this.attack.cooldown = cooldown;
            timer = 0;
        }
    }

    public float GetAttackCooldown() {
        return attack.cooldown;
    }

    /// <summary>
    /// Returns a reference to the Cherry currently being targeted by the Topping. Returns null if no Cherries are
    /// being targeted.
    /// </summary>
    /// <returns>
    /// A reference to the targeted Cherry as a GameObject
    /// </returns>
    public GameObject GetTargetedCherry() {
        return this.targetedCherry;
    }

    /// <summary>
    /// Returns true if this Topping is stunned and false otherwise.
    /// </summary>
    /// <returns></returns>
    public bool IsStunned() {
        return this.isStunned;
    }

}
