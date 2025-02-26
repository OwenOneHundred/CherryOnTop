using UnityEngine;

/**
 * Script that handles the attack cycle for a Topping. When a Topping is initialized, it must be given an attack type
 * using the SetAttack method.
 */
public class AttackManager : MonoBehaviour
{
    // Represents the type of attack to use, which holds information about the Topping's attack behaviour
    [SerializeField]
    ToppingAttack attack;

    // Keeps track of the current targeted Cherry. Should be null if there are no Cherries in range
    [SerializeField]
    GameObject targetedCherry;

    // Keeps track of when the last attack was to time the next one
    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (this.attack != null) {
            this.attack.OnStart();
        }
        Debug.Log("Topping attack system initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedCherry != null) {
            if (timer >= attack.cooldown) {
                timer = 0;
                attack.OnCycle(this.targetedCherry);
            }
            timer += Time.deltaTime;
        } else {
            timer = 0;
        }
    }

    // Note to Tony from Owen: These methods are all private (private is the default)
    // They are not called from within this class either so they are unusable. They should probably be public.
    // Your Visual Studio Code would tell you that if you had it set up.

    /**
     * Switches targets to a new Cherry or to null if appropriate.
     */
    void UpdateTargetedCherry(GameObject targetedCherry) {
        if (this.targetedCherry != targetedCherry) {
            this.targetedCherry = targetedCherry;
            if (this.targetedCherry != null) {
                attack.OnNewCherryFound(this.targetedCherry);
            }
        }       
    }

    /**
     * Sets this Topping's attack type to a specified ToppingAttack.
     */
    void SetAttack(ToppingAttack attack) {
        if (this.attack != attack) {
            this.attack = attack;
            this.attack.OnStart();
        }
    }

    /** 
     * Sets this Topping's attack cooldown to a specified value.
     */
    void SetAttackCooldown(float cooldown) {
        this.attack.cooldown = cooldown;
    }

    /**
     * Returns a reference to the Cherry currently being targeted by the Topping. Returns null if no Cherries are
     * being targeted.
     */
    GameObject GetTargetedCherry() {
        return this.targetedCherry;
    }

}
