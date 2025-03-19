using UnityEngine;

/// <summary>
/// A special kind of projectile that explodes on impact with a Cherry.
/// </summary>
public class ExplodingProjectile : Projectile
{
    public override void OnHitCherry(CherryHitbox ch) {
        Explode();
    }

    private void Explode() {
        Destroy(gameObject);
    }
}
