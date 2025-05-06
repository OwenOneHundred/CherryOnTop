using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/Artillery")]
public class ArtilleryAttack : ProjectileAttack
{
    public override void OnCycle(GameObject targetedCherry)
    {
        AttackCherry(targetedCherry);
    }

    private void AttackCherry(GameObject targetedCherry)
    {
        GameObject newProjectile = SpawnProjectile(this.projectile, toppingObj.transform.position, Vector3.zero, Quaternion.identity, this.damage);
        newProjectile.GetComponent<Projectile>().SetTarget(targetedCherry.transform.position);
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public override void OnStart()
    {
        
    }

    public override int GetVisibleDamage()
    {
        return projectile.GetComponent<ArtilleryProjectile>().shockwaveDamage;
    }
}
