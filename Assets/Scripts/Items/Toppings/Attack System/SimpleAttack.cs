using UnityEngine;

/// <summary>
/// Simple attack type that shoots a projectile in the direction of the target Cherry.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Simple Attack")]
public class SimpleAttack : ProjectileAttack
{
    public override void OnStart() {
        
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        Debug.Log("New Cherry Targeted");
    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
    }

    public override void SpawnProjectile(GameObject projectile, Vector3 position, Vector3 velocity, Quaternion rotation, int damage) {
        GameObject newProjectile = Instantiate(projectile, position, rotation);
        newProjectile.GetComponent<Rigidbody>().linearVelocity = velocity;
        newProjectile.GetComponent<Projectile>().damage = damage;

        // Destroy the projectile after 8 seconds in case it misses the target
        Destroy(newProjectile, 8);
    }

    /// <summary>
    /// Fires a projectile in the direction of the current targeted Cherry
    /// </summary>
    /// <param name="targetedCherry"></param>
    private void AttackCherry(GameObject targetedCherry) {
        SpawnProjectile(this.projectile, topping.transform.position, FindTargetVector(targetedCherry), Quaternion.identity, this.damage);
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
}
