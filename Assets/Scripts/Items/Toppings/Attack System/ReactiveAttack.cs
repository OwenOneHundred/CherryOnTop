using UnityEngine;

/// <summary>
/// A ToppingAttack that fires a burst of projectiles whenever it begins targeting a new Cherry.
/// </summary>
public class ReactiveAttack : ToppingAttack
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

    // Represents the time between shots in the burst
    [SerializeField]
    float firingRate;

    public override void OnStart() {
        Debug.Log("Reactive attack with a firing rate of " + this.firingRate + " seconds assigned to topping " + this.topping.name + ".");
    }
}
