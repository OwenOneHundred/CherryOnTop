using UnityEngine;

/**
 * Script that handles the attack cycle for a Topping. When a Topping is initialized, it must be given an attack type
 * using GetComponent<AttackManager>().SetAttack(attack);
 */
public class AttackManager : MonoBehaviour
{
    // Represents the type of attack to use, which holds information about the Topping's attack cooldown behaviour
    [SerializeField]
    ToppingAttack attack;

    // Keeps track of the current targeted cherry. Should be null if there are no cherries in range
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

    void UpdateTargetedCherry(GameObject targetedCherry) {
        if (this.targetedCherry != targetedCherry) {
            this.targetedCherry = targetedCherry;
            attack.OnNewCherryFound(this.targetedCherry);
        }       
    }

    void SetAttack(ToppingAttack attack) {
        this.attack = attack;
        if (this.attack != null) {
            this.attack.OnStart();
        }
    }

    void SetAttackCooldown(float cooldown) {
        this.attack.cooldown = cooldown;
    }

    GameObject GetTargetedCherry() {
        return this.targetedCherry;
    }

}
