using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "CherryDebuff/Burn")]
public class BurnEffect : CherryDebuff
{
    [SerializeField] float dps = 3;
    CherryHitbox cherryHitbox;
    [SerializeField] GameObject fireParticleSystemPrefab;
    GameObject firePSObj;
    public override void EveryFrame()
    {
        cherryHitbox.TakeDamage(dps * Time.deltaTime, null);
    }

    public override void OnAdded(GameObject cherry)
    {
        SoundEffectManager.sfxmanager.PlayOneShot(onAppliedSFX);
        
        // Set cherry field to the GameObject cherry argument
        this.cherry = cherry;
        cherryHitbox = cherry.GetComponent<CherryHitbox>();

        firePSObj = Instantiate(fireParticleSystemPrefab, cherry.transform.position, Quaternion.identity, cherry.transform);
        ParticleSystem.ShapeModule shape = firePSObj.GetComponent<ParticleSystem>().shape;
        shape.mesh = cherry.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
        firePSObj.transform.localScale = cherry.transform.GetChild(0).lossyScale;
    }

    public override void OnRemoved(GameObject cherry)
    {
        firePSObj.transform.parent = null;
        firePSObj.GetComponent<ParticleSystem>().Stop();
        Destroy(firePSObj, 3);
    }
}
