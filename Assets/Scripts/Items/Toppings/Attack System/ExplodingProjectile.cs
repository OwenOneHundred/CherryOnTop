using UnityEngine;

public class ExplodingProjectile : Projectile
{
    public override void OnHitCherry(CherryHitbox ch) {
        Explode();
    }

    private void Explode() {
        Destroy(gameObject);
    }
}
