using UnityEngine;

/**
 * Script that attacks a targeted cherry at a specified rate.
 */
public class ToppingAttack : MonoBehaviour
{
    // Keeps track of the current targeted cherry. Should be null if there are no cherries in range
    GameObject targetedCherry;
    // Represents the number of seconds to wait before attacking
    float attackRate;
    // Keeps track of when the last attack was to time the next one
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.targetedCherry = null;
        this.attackRate = 0;
        this.timer = 0;
        Debug.Log("Topping attack system initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedCherry != null) {
            if (timer >= attackRate) {
                attackCherry();
                timer = 0;
            }
            timer += Time.deltaTime;
        } else {
            timer = 0;
        }
    }

    void setTargetedCherry(GameObject targetedCherry) {
        this.targetedCherry = targetedCherry;
    }

    void setAttackRate(int attackRate) {
        this.attackRate = attackRate;
    }

    GameObject getTargetedCherry() {
        return this.targetedCherry;
    }

    void attackCherry() {
        // to do
    }
}
