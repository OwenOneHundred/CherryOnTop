using UnityEngine;
using System.Collections;

/// <summary>
/// A ToppingAttack that fires a burst of projectiles whenever it begins targeting a new Cherry.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Reactive Attack")]
public class ReactiveAttack : ToppingAttack
{
    // Stores a reference to the prefab used as the projectile
    [SerializeField]
    GameObject projectile;

    // Represents the speed of the projectiles shot by this ToppingAttack
    [SerializeField]
    float projectileSpeed;

    // Represents the total number of projectiles to shoot towards the target Cherry
    [SerializeField]
    int burstQuantity;

    // Represents the time between shots in the burst
    [SerializeField]
    float firingDelay;

    // Represents whether the Topping is free to attack (true) or is hindered by its cooldown (false)
    private bool canAttack;

    public override void OnStart() {
        Debug.Log("Reactive attack with a cooldown of " + this.cooldown + " seconds assigned to topping " + this.topping.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        if (canAttack) {
            for (int i = 0; i < burstQuantity; i++) {
                this.topping.GetComponent<AttackManager>().StartCoroutine(WaitAndAttack(newTargetedCherry, i * firingDelay));
            }
            canAttack = false;
        }
    }

    public override void OnCycle(GameObject targetedCherry) {
        canAttack = true;
    }

    /// <summary>
    /// Fires a stream of projectiles in the direction of the current targeted Cherry
    /// </summary>
    /// <param name="targetedCherry"></param>
    private void AttackCherry(GameObject targetedCherry) {
        GameObject newProjectile = Instantiate(this.projectile, topping.transform.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().linearVelocity = FindTargetVector(targetedCherry);

        // Destroy the projectile after 8 seconds in case it misses the target
        Destroy(newProjectile, 8);
    }

    /// <summary>
    /// Finds and returns the vector pointing from the Topping to the target Cherry with a magnitude corresponding to
    /// projectileSpeed
    /// </summary>
    /// <param name="targetedCherry"></param>
    /// <returns>
    /// A Vector3 object
    /// </returns>
    private Vector3 FindTargetVector(GameObject targetedCherry){
        if (targetedCherry == null) {
            return new Vector3(0, projectileSpeed, 0);
        }

        Vector3 targetDirection = targetedCherry.transform.position - topping.transform.position;
        targetDirection.Normalize();
        return projectileSpeed * targetDirection;
    }

    /// <summary>
    /// Waits a specified number of seconds and then attacks the specified targeted Cherry
    /// </summary>
    /// <param name="targetedCherry"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator WaitAndAttack(GameObject targetedCherry, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        AttackCherry(targetedCherry);
    }
}
