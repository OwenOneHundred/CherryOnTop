using UnityEngine;

[CreateAssetMenu(menuName ="CherryDebuff/Freeze")]
public class FreezeDebuff : CherryDebuff
{
    readonly Vector3 iceBlockManualAdjustment = new Vector3(0f, 0.035f, 0.058f);
    [SerializeField] GameObject iceBlock;
    GameObject iceBlockSpawned;
    public override void EveryFrame()
    {

    }

    public override void OnAdded(GameObject cherry)
    {
        SoundEffectManager.sfxmanager.PlayOneShot(onAppliedSFX);
        iceBlockSpawned = Instantiate(iceBlock, cherry.transform.position, Quaternion.identity);
        Vector3 lossyScale = cherry.GetComponentInChildren<MeshRenderer>().transform.lossyScale;
        iceBlockSpawned.transform.localScale = lossyScale;
        iceBlockSpawned.transform.parent = cherry.transform;
        iceBlockSpawned.transform.SetAsLastSibling();
        Vector3 iceBlockAdjustmentAdjusted = (iceBlockManualAdjustment / 0.3f) * lossyScale.x;
        iceBlockSpawned.transform.position += iceBlockAdjustmentAdjusted;
    }

    public override void OnRemoved(GameObject cherry)
    {
        if (iceBlockSpawned != null) { Destroy(iceBlockSpawned); }
    }
}
