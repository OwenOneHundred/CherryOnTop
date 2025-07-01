using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "CherryDebuff/Burn")]
public class BurnEffect : CherryDebuff
{
    CherryHitbox cherryHitbox;
    [SerializeField] GameObject fireParticleSystemPrefab;
    GameObject firePSObj;
    public override void EveryFrame()
    {
        cherryHitbox.TakeDamage(dps * Time.deltaTime, null);
    }

    public override void OnAdded(GameObject cherry)
    {
        SoundEffectManager.sfxmanager.TryPlayOneShot(onAppliedSFX);
        
        // Set cherry field to the GameObject cherry argument
        this.cherry = cherry;
        cherryHitbox = cherry.GetComponent<CherryHitbox>();

        firePSObj = Instantiate(fireParticleSystemPrefab, cherry.transform.position, Quaternion.identity, cherry.transform);
        firePSObj.transform.localScale = cherry.transform.GetChild(0).localScale;

        // actual scrub code lmao. Breaks open/close bc this number is balancing related, breaks responsibility separation
        // but look man, it's literally ONE line and it does something that requires 2 classes
        // so if you're reading this SHHHHHHHHHHH
        if (cherry.GetComponent<DebuffManager>().HasDebuffType(DebuffType.fondue))
        {
            dps *= 2;
        }
    }

    public override void OnRemoved(GameObject cherry)
    {
        firePSObj.transform.parent = null;
        firePSObj.GetComponent<ParticleSystem>().Stop();
        Destroy(firePSObj, 3);
    }
}
