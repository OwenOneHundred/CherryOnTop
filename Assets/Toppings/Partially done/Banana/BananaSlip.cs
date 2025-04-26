using UnityEngine;

[CreateAssetMenu(menuName = "CherryDebuff/Banana Slip")]
public class BananaSlip : CherryDebuff
{
    public override void EveryFrame()
    {
        
    }

    public override void OnAdded(GameObject cherry)
    {
        SoundEffectManager.sfxmanager.PlayOneShot(onAppliedSFX);
    }

    public override void OnRemoved(GameObject cherry)
    {

    }
}
