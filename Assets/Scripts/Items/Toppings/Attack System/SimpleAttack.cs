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
        
    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
    }

    /// <summary>
    /// Fires a projectile in the direction of the current targeted Cherry
    /// </summary>
    /// <param name="targetedCherry"></param>
    private void AttackCherry(GameObject targetedCherry) {
        SpawnProjectile(this.projectile, toppingFirePointObj.transform.position, FindTargetVector(targetedCherry), Quaternion.identity, this.damage);
    }

    /// <summary>
    /// Finds and returns the vector pointing from the Topping to the target Cherry with a magnitude corresponding to
    /// projectileSpeed
    /// </summary>
    /// <param name="targetedCherry"></param>
    /// <returns>
    /// A Vector3 object
    /// </returns>
    protected Vector3 FindTargetVector(GameObject targetedCherry){
        if (targetedCherry == null) {
            return new Vector3(0, projectileSpeed, 0);
        }

        Vector3 targetDirection = targetedCherry.transform.position - toppingFirePointObj.transform.position;
        targetDirection.Normalize();
        return projectileSpeed * targetDirection;
    }
}
