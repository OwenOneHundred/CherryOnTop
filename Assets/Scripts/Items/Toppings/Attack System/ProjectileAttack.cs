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

    [SerializeField] AudioFile fireSound;

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
    public virtual void SpawnProjectile(GameObject projectile, Vector3 position, Vector3 velocity, Quaternion rotation, int damage)
    {
        GameObject newProjectile = Instantiate(projectile, position, rotation);
        newProjectile.GetComponent<Rigidbody>().linearVelocity = velocity;
        newProjectile.GetComponent<Projectile>().damage = damage;
        newProjectile.GetComponent<Projectile>().owner = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;

        // Destroy the projectile after 8 seconds in case it misses the target
        Destroy(newProjectile, 8);

        if (fireSound != null) { SoundEffectManager.sfxmanager.PlayOneShot(fireSound);}

        CustomProjectileActions(newProjectile);
    }

    /// <summary>
    /// Automatically called by SpawnProjectile. Override this instead of SpawnProjectile to add additional actions if possible.
    /// </summary>
    /// <param name="projectile"></param>
    public virtual void CustomProjectileActions(GameObject projectile)
    {

    }
}
