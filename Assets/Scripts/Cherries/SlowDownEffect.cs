using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "Slow")]
public class SlowDownEffect : CherryDebuff
{
    public override void EveryFrame()
    {
        // This is where effects would deal damage and operate logic.

        throw new System.NotImplementedException();
    }

    public override void OnAdded(GameObject cherry)
    {
        // Add VFX on cherry

        // Set cherry field to the GameObject cherry argument
        throw new System.NotImplementedException();
    }

    public override void OnRemoved()
    {
        // Remove VFX from cherry

        // Should not error when object is null
        throw new System.NotImplementedException();
    }
}
