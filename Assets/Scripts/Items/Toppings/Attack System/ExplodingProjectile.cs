using UnityEngine;

/// <summary>
/// A special kind of projectile that explodes on impact with a Cherry.
/// </summary>
public class ExplodingProjectile : Projectile
{
    [SerializeField]
    GameObject shockwave;

    [SerializeField]
    GameObject particleEmitter;

    public override void OnHitCherry(CherryHitbox ch) {
        Destroy(gameObject);
        Explode();
    }

    private void Explode() {
        GameObject newShockwave = Instantiate(shockwave, transform.position, Quaternion.identity);
        GameObject newParticleEmitter = Instantiate(particleEmitter, transform.position, Quaternion.identity);

        Destroy(newParticleEmitter, 5);
    }
}
