using UnityEngine;
using System.Collections;

/// <summary>
/// A ToppingAttack that fires a burst of projectiles whenever it begins targeting a new Cherry.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Reactive Attack")]
public class ReactiveAttack : ProjectileAttack
{
    // Represents the total number of projectiles to shoot towards the target Cherry
    [SerializeField]
    int burstQuantity;

    // Represents the time between shots in the burst
    [SerializeField]
    float firingDelay;

    // Represents whether the current targeted Cherry has been attacked at least once so far.
    private bool attackSuccessful = false;

    public override void OnStart() {
        Debug.Log("Reactive attack with a cooldown of " + this.cooldown + " seconds assigned to topping " + this.toppingObj.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        attackSuccessful = false;
    }

    public override void OnCycle(GameObject targetedCherry) {
        if (!attackSuccessful) {
            for (int i = 0; i < burstQuantity; i++) {
                this.toppingObj.GetComponent<AttackManager>().StartCoroutine(DelayedAttack(targetedCherry, i * firingDelay));
            }
            attackSuccessful = true;
        }
    }

    /// <summary>
    /// Fires a stream of projectiles in the direction of the current targeted Cherry
    /// </summary>
    /// <param name="targetedCherry"></param>
    private void AttackCherry(GameObject targetedCherry) {
        SpawnProjectile(this.projectile, toppingObj.transform.position, FindTargetVector(targetedCherry), Quaternion.identity, this.damage);
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

        Vector3 targetDirection = targetedCherry.transform.position - toppingObj.transform.position;
        targetDirection.Normalize();
        return projectileSpeed * targetDirection;
    }

    /// <summary>
    /// Waits a specified number of seconds and then attacks the specified targeted Cherry
    /// </summary>
    /// <param name="targetedCherry"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator DelayedAttack(GameObject targetedCherry, float delay) {
        yield return new WaitForSeconds(delay);
        AttackCherry(targetedCherry);
    }
}
