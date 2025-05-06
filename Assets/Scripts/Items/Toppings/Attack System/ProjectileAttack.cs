using UnityEngine;

/// <summary>
/// An abstract class that represents ToppingAttacks which shoot projectiles.
/// </summary>
public abstract class ProjectileAttack : ToppingAttack
{
    // Stores a reference to the prefab used as the projectile
    public GameObject projectile;

    // Represents the speed of the projectiles shot by this ToppingAttack
    public float projectileSpeed;

    [SerializeField] protected AudioFile fireSound;
    public float bulletLifetime = 8;

    /// <summary>
    /// Spawn a projectile with an initial position, velocity, rotation, and damage.
    /// </summary>
    /// <param name="projectile">
    /// A reference to the prefab used to model the projectile.
    /// </param>
    /// <param name="position"></param>
    /// <param name="velocity"></param>
    /// <param name="rotation"></param>
    /// <param name="damage"></param>
    public virtual GameObject SpawnProjectile(GameObject projectile, Vector3 position, Vector3 velocity, Quaternion rotation, int damage)
    {
        GameObject newProjectile = Instantiate(projectile, position, rotation);
        newProjectile.GetComponent<Rigidbody>().linearVelocity = velocity;
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        projectileScript.damage = damage;
        projectileScript.owner = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;

        // Destroy the projectile after bulletLifetime seconds in case it misses the target
        Destroy(newProjectile, bulletLifetime);

        if (fireSound != null && fireSound.clip != null) { SoundEffectManager.sfxmanager.PlayOneShot(fireSound);}

        CustomProjectileActions(newProjectile);

        return newProjectile;
    }

    /// <summary>
    /// Automatically called by SpawnProjectile. Override this instead of SpawnProjectile to add additional actions if possible.
    /// </summary>
    /// <param name="projectile"></param>
    public virtual void CustomProjectileActions(GameObject projectile)
    {

    }

    public override string GetPierce()
    {
        return projectile.GetComponent<Projectile>().maxHits + "";
    }
}
