using UnityEngine;
using System.Collections;

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

    // Keeps track of the current targeted Cherry. Should be null if there are no Cherries in range
    [SerializeField]
    GameObject targetedCherry;

    // Keeps track of when the last attack was to time the next one
    private float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.attack = Instantiate(attackTemplate);
        this.attack.topping = gameObject;
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
    /// Sets this Topping's attack cooldown to a specified value.
    /// </summary>
    /// <param name="cooldown"></param>
    public void SetAttackCooldown(float cooldown) {
        this.attack.cooldown = cooldown;
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

}
