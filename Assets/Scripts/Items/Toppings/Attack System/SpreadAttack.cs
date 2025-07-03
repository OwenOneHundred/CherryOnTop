using UnityEngine;

/// <summary>
/// A ToppingAttack that shoots multiple projectiles towards a target Cherry, with a specified quantity and
/// spread angle.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Spread Attack")]
public class SpreadAttack : ProjectileAttack
{
    // Represents the total number of projectiles to shoot towards the target Cherry
    [SerializeField]
    int quantity;

    // Represents the total spread between the projectiles, in radians
    [SerializeField]
    double spreadAngle;

    public override void OnStart() {
        
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {

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

            double offsetAngle = 0;
            if (quantity != 1) {
                offsetAngle = ((double) i / (quantity - 1) - 0.5) * spreadAngle;
            }

            SpawnProjectile(this.projectile, toppingFirePointObj.transform.position, FindTargetVector(targetedCherry, offsetAngle), Quaternion.identity, (int)this.damage);
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
        Vector3 targetDirection = targetedCherry.transform.position - toppingFirePointObj.transform.position;
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
