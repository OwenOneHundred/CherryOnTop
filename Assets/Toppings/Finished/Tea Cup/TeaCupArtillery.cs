using UnityEngine;

public class SplashArtilleryProjectile : ArtilleryProjectile
{
    [SerializeField] float lifetime = 2;
    protected override void Explode() {
        if (shockwave != null)
        {
            GameObject newSplash = Instantiate(shockwave, transform.position, Quaternion.identity);
            SplashObj splashComp = newSplash.GetComponent<SplashObj>();
            splashComp.lifetime = lifetime;

            Projectile projectileComp = newSplash.GetComponent<Projectile>();
            projectileComp.damage = damage;
            projectileComp.owner = owner;
            
            if (cameraShakeLength > 0)
            {
                Camera.main.transform.parent.GetComponent<CameraControl>().ApplyCameraShake(cameraShakeLength, cameraShakeViolence);
            }
            if (explodeSound.clip != null) { SoundEffectManager.sfxmanager.PlayOneShot(explodeSound); }
        }
        
        Destroy(gameObject);
    }
}
