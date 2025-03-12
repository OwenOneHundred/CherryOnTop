using UnityEngine;

/// <summary>
/// A ToppingAttack that shoots multiple projectiles towards a target Cherry, with a specified quantity and
/// spread angle.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Spread Attack")]
public class SpreadAttack : ToppingAttack
{
    // Stores a reference to the prefab used as the projectile
    [SerializeField]
    GameObject projectile;

    // Represents the speed of the projectiles shot by this ToppingAttack
    [SerializeField]
    float projectileSpeed;

    // Represents the total number of projectiles to shoot towards the target Cherry
    [SerializeField]
    int quantity;

    // Represents the total spread between the projectiles, in radians
    [SerializeField]
    double spreadAngle;

    public override void OnStart() {
        Debug.Log("Spread attack with a cooldown of " + this.cooldown + " assigned to topping " + this.topping.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
         Debug.Log("New Cherry Targeted");
    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
    }

    /// <summary>
    /// Shoots one or more projectiles in the direction of the targeted Cherry
    /// </summary>
    /// <param name="targetedCherry"></param>
    private void AttackCherry(GameObject targetedCherry) {
        for (int i = 0; i < quantity; i++) {
            GameObject newProjectile = Instantiate(this.projectile, topping.transform.position, Quaternion.identity);

            double offsetAngle = 0;
            if (quantity != 1) {
                offsetAngle = ((double) i / (quantity - 1) - 0.5) * spreadAngle;
            }

            newProjectile.GetComponent<Rigidbody>().linearVelocity = FindTargetVector(targetedCherry, offsetAngle);

            // Destroy the projectile after 8 seconds in case it misses the target
            Destroy(newProjectile, 8);
        }
    }

    /// <summary>
    /// Finds and returns the vector pointing from the Topping to the target Cherry with a certain offset and 
    /// with a magnitude corresponding to projectileSpeed
    /// </summary>
    /// <param name="targetedCherry"></param>
    /// <returns> 
    /// A Vector3 object
    /// </returns>
    private Vector3 FindTargetVector(GameObject targetedCherry, double offsetAngle){
        if (targetedCherry == null) {
            return new Vector3(0, projectileSpeed, 0);
        }

        // Finds and normalizes the direction directly to the Cherry
        Vector3 targetDirection = targetedCherry.transform.position - topping.transform.position;
        targetDirection.Normalize();

        // Calculates the offset vector based on offsetAngle
        Vector3 offsetVector = new Vector3(-1 * targetDirection.z, 0, targetDirection.x);
        offsetVector *= Mathf.Sin((float) offsetAngle);
        offsetVector -= (1 - Mathf.Cos((float) offsetAngle)) * targetDirection;

        // Adds the offset to the original vector and renormalizes
        targetDirection += offsetVector;
        targetDirection.Normalize();
        
        return projectileSpeed * targetDirection;
    }
}
