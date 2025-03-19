using UnityEngine;

/// <summary>
/// An abstract class that represents ToppingAttacks which shoot projectiles.
/// </summary>
public abstract class ProjectileAttack : ToppingAttack
{
    // Stores a reference to the prefab used as the projectile
    [SerializeField]
    GameObject projectile;

    // Represents the speed of the projectiles shot by this ToppingAttack
    [SerializeField]
    float projectileSpeed;
    
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
    public abstract void SpawnProjectile(GameObejct projectile, Vector3 position, Vector3 velocity, Quaternion rotation, float damage);
}
