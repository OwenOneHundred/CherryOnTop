using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "CherryDebuff/Slow")]
public class SlowDownEffect : CherryDebuff
{
    public override void EveryFrame()
    {
        // Slow down effect doesn't really do anything every frame

    }

    public override void OnAdded(GameObject cherry)
    {
        // Add VFX to cherry

        // Set cherry field to the GameObject cherry argument
        this.cherry = cherry;
        movementSpeedMultiplier = 0.5f;
    }

    public override void OnRemoved(GameObject cherry)
    {
        // Remove VFX from cherry

        // Should not error when object is null
        throw new System.NotImplementedException();
    }
}
