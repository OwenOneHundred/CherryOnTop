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
    float shockwaveRange;

    [SerializeField]
    GameObject particleEmitter;

    [SerializeField] float cameraShakeViolence = 1;
    [SerializeField] float cameraShakeLength = 0;
    [SerializeField] AudioFile onHitSound;

    public override void OnHitCherry(CherryHitbox ch) {
        Destroy(gameObject);
        Explode();
    }

    private void Explode() {
        GameObject newShockwave = Instantiate(shockwave, transform.position, Quaternion.identity);
        newShockwave.GetComponent<Shockwave>().range = shockwaveRange;
        newShockwave.GetComponent<Shockwave>().SetDamage(shockwaveDamage);
        if (particleEmitter != null)
        {
            GameObject newParticleEmitter = Instantiate(particleEmitter, transform.position, Quaternion.identity);

            Destroy(newParticleEmitter, 5);
        }
        if (cameraShakeLength > 0)
        {
            Camera.main.transform.parent.GetComponent<CameraControl>().ApplyCameraShake(cameraShakeLength, cameraShakeViolence);
        }
        SoundEffectManager.sfxmanager.PlayOneShot(onHitSound);
    }
}
