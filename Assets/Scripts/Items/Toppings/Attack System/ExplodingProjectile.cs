using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// A special kind of projectile that explodes on impact with a Cherry.
/// </summary>
public class ExplodingProjectile : Projectile
{
    [SerializeField]
    protected GameObject shockwave;

    [SerializeField]
    int shockwaveDamage;

    [SerializeField]
    float shockwaveSpeed = 1;

    [SerializeField]
    float shockwaveRange;
    [SerializeField] bool explodeOnTerrain = false;
    [SerializeField] bool explodeOnCherry = true;

    public override void OnHitCherry(CherryHitbox ch) {
        if (explodeOnCherry)
        {
            Debug.Log("here");
            Explode();
        }
    }

    public override void OnTriggerEnter(UnityEngine.Collider other)
    {
        base.OnTriggerEnter(other);
        Debug.Log("here2");
        if (explodeOnTerrain)
        {
            if (other.gameObject.layer == 7)
            {
                Explode();
            }
        }
    }

    [SerializeField] protected float cameraShakeViolence = 1;
    [SerializeField] protected float cameraShakeLength = 0;
    [SerializeField] protected AudioFile explodeSound;

    protected virtual void Explode() {
        if (shockwave != null)
        {
            GameObject newShockwave = Instantiate(shockwave, transform.position, Quaternion.identity);
            Shockwave shockwaveComponent = newShockwave.GetComponent<Shockwave>();
            newShockwave.GetComponent<Shockwave>().speed = shockwaveSpeed;
            shockwaveComponent.range = shockwaveRange;
            shockwaveComponent.SetDamage(shockwaveDamage);
            shockwaveComponent.owner = owner;
            shockwaveComponent.speed = shockwaveSpeed;
            
            if (cameraShakeLength > 0)
            {
                Camera.main.transform.parent.GetComponent<CameraControl>().ApplyCameraShake(cameraShakeLength, cameraShakeViolence);
            }
            if (explodeSound.clip != null) { SoundEffectManager.sfxmanager.PlayOneShot(explodeSound); }
        }
        
        Destroy(gameObject);
    }
}
