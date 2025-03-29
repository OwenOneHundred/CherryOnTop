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
    float shockwaveSpeed = 1;

    [SerializeField]
    float shockwaveRange;

    public override void OnHitCherry(CherryHitbox ch) {
        Destroy(gameObject);
        Explode();
    }

    [SerializeField] float cameraShakeViolence = 1;
    [SerializeField] float cameraShakeLength = 0;
    [SerializeField] AudioFile onHitSound;

    private void Explode() {
        GameObject newShockwave = Instantiate(shockwave, transform.position, Quaternion.identity);
        Shockwave shockwaveComponent = newShockwave.GetComponent<Shockwave>();
        newShockwave.GetComponent<Shockwave>().speed = shockwaveSpeed;
        shockwaveComponent.range = shockwaveRange;
        shockwaveComponent.SetDamage(shockwaveDamage);
        shockwaveComponent.owner = owner;
        
        if (cameraShakeLength > 0)
        {
            Camera.main.transform.parent.GetComponent<CameraControl>().ApplyCameraShake(cameraShakeLength, cameraShakeViolence);
        }
        SoundEffectManager.sfxmanager.PlayOneShot(onHitSound);
    }
}
