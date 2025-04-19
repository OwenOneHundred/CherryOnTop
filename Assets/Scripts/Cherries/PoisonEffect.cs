using System.Net;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "CherryDebuff/PoisonEffect")]
public class PoisonEffect : CherryDebuff
{
    [SerializeField] float dps = 3;
    CherryHitbox cherryHitbox;
    [SerializeField] GameObject poisonParticleSystemPrefab;
    GameObject poisonPSObj;

    public override void EveryFrame()
    {
        // Cherries take damage every tick
        cherryHitbox.TakeDamage(dps * Time.deltaTime, null);
    }

    public override void OnAdded(GameObject cherry)
    {
        SoundEffectManager.sfxmanager.PlayOneShot(onAppliedSFX);
        
        // Adding particles to cherry
        this.cherry = cherry;
        poisonPSObj = Instantiate(poisonPSObj, cherry.transform.position, Quaternion.identity, cherry.transform);
        poisonPSObj.transform.localScale = cherry.transform.GetChild(0).localScale;
    }

    public override void OnRemoved(GameObject cherry)
    {
        // Remove VFX from cherry
        poisonPSObj.transform.parent = null;
        poisonPSObj.GetComponent<ParticleSystem>().Stop();
        Destroy(poisonPSObj, 2);
    }
}
