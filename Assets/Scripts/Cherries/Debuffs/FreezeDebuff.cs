using UnityEngine;

[CreateAssetMenu(menuName ="CherryDebuff/Freeze")]
public class FreezeDebuff : CherryDebuff
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
