using UnityEngine;

/**
 * Simple attack type that shoots a projectile in the direction of the target Cherry.
 */
public class SimpleToppingAttack : ToppingAttack
{
    // Stores a reference to the prefab used as the projectile
    [SerializeField]
    GameObject projectile;

    // Represents the speed of the projectiles shot by this ToppingAttack
    [SerializeField]
    float projectileSpeed;

    public SimpleToppingAttack(GameObject topping, float cooldown, GameObject projectile, float projectileSpeed) {
        this.topping = topping;
        this.cooldown = cooldown;
        this.projectile = projectile;
        this.projectileSpeed = projectileSpeed;
    }

    public override void OnStart() {
        Debug.Log("Simple attack with a cooldown of " + this.cooldown + " seconds assigned to topping " + this.topping.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        // to do
        Debug.Log("OnNewCherryFound");
    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
    }

    /**
     * Fires a projectile in the direction of the current targeted Cherry
     */
    private void AttackCherry(GameObject targetedCherry) {
        GameObject projectile = Instantiate(this.projectile, topping.transform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().linearVelocity = FindTargetVector(targetedCherry);

        Destroy(projectile, 8);
    }

    /**
     * Finds and returns the vector pointing from the Topping to the target Cherry with a magnitude corresponding to
     * projectileSpeed
     */
    private Vector3 FindTargetVector(GameObject targetedCherry){
        if (targetedCherry == null) {
            return new Vector3(0, projectileSpeed, 0);
        }

        Vector3 targetDirection = targetedCherry.transform.position - topping.transform.position;
        targetDirection.Normalize();
        return projectileSpeed * targetDirection;
    }
}
