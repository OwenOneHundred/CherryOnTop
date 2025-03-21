using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "CherryDebuff/Slow")]
public class SlowDownEffect : CherryDebuff
{
    private float effectDuration; // this should be in the parent
    private float timeSinceTick;

    public override void EveryFrame() // this would result in cherries rapidly slowing down and then moving backward
    {
        // Cherries speed slows down every tick
        this.timeSinceTick += Time.deltaTime;
        if (this.timeSinceTick >= this.effectDuration)
        {
            this.timeSinceTick = 0;
            movementSpeedMultiplier -= 0.1f;
        }
    }

    public override void OnAdded(GameObject cherry)
    {
        // Add VFX to cherry

        // Set cherry field to the GameObject cherry argument
        this.cherry = cherry;
        movementSpeedMultiplier = 1f;
        this.effectDuration = 10f;
    }

    public override void OnRemoved(GameObject cherry)
    {
        // Remove VFX from cherry

        // Should not error when object is null
        throw new System.NotImplementedException();
    }
}
