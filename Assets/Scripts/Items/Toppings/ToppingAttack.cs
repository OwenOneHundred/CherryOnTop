using UnityEngine;

/**
 * Script that attacks a targeted cherry at a specified rate.
 */
public class ToppingAttack : MonoBehaviour
{
    // keeps track of the current targeted cherry. Should be null if there are no cherries in range
    GameObject targetedCherry;
    // represents the number of frames to wait before attacking
    int attackRate;
    // keeps track of when the last attack was to time the next one
    int timer;

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
        if(targetedCherry != null) {
            if (timer == attackRate) {
                attackCherry();
                timer = 0;
            }
            timer++;
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
