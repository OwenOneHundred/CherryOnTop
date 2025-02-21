using UnityEngine;

/**
 * Script that attacks a targeted cherry at a specified rate.
 */
public class ToppingAttack : MonoBehaviour
{
    // Keeps track of the current targeted cherry. Should be null if there are no cherries in range
    [SerializeField]
    GameObject targetedCherry;

    // Stores a reference to the prefab used as the projectile
    [SerializeField]
    GameObject projectile;

    // Represents the number of seconds to wait before attacking
    [SerializeField]
    float attackRate;

    // Represents the speed of the projectiles shot by this topping
    [SerializeField]
    float projectileSpeed;

    // Keeps track of when the last attack was to time the next one
    float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Topping attack system initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        if (targetedCherry != null) {
            if (timer >= attackRate) {
                AttackCherry();
                timer = 0;
            }
            timer += Time.deltaTime;
        } else {
            timer = 0;
        }
    }

    void SetTargetedCherry(GameObject targetedCherry) {
        this.targetedCherry = targetedCherry;
    }

    void SetAttackRate(float attackRate) {
        this.attackRate = attackRate;
    }

    void SetProjectileSpeed(float projectileSpeed) {
        this.projectileSpeed = projectileSpeed;
    }

    GameObject GetTargetedCherry() {
        return this.targetedCherry;
    }

    void AttackCherry() {
        GameObject projectile = Instantiate(this.projectile, transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().linearVelocity = FindTargetVector();
    }

    Vector3 FindTargetVector(){
        if (targetedCherry == null) {
            return new Vector3(0, projectileSpeed, 0);
        }

        Vector3 targetDirection = targetedCherry.transform.position - transform.position;
        targetDirection.Normalize();
        return projectileSpeed * targetDirection;
    }

}
