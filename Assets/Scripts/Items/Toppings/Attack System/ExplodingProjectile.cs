using UnityEngine;

/// <summary>
/// A special kind of projectile that explodes on impact with a Cherry.
/// </summary>
public class ExplodingProjectile : Projectile
{
    [SerializeField]
    GameObject shockwave;

    [SerializeField]
    int shockwaveDamage;

    [SerializeField]
    float shockwaveSpeed;

    [SerializeField]
    float shockwaveRange;

    //[SerializeField]
    //GameObject particleEmitter;

    public override void OnHitCherry(CherryHitbox ch) {
        Destroy(gameObject);
        Explode();
    }

    private void Explode() {
        GameObject newShockwave = Instantiate(shockwave, transform.position, Quaternion.identity);
        newShockwave.GetComponent<Shockwave>().speed = shockwaveSpeed;
        newShockwave.GetComponent<Shockwave>().range = shockwaveRange;
        newShockwave.GetComponent<Shockwave>().SetDamage(shockwaveDamage);
        //GameObject newParticleEmitter = Instantiate(particleEmitter, transform.position, Quaternion.identity);

        //Destroy(newParticleEmitter, 5);
    }
}
