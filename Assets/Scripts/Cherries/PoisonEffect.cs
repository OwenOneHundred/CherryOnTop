using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "CherryDebuff/Poison")]
public class PoisonEffect : CherryDebuff
{
    public override void EveryFrame()
    {
        // This is where effects would deal damage and operate logic.
        this.cherry.GetComponent<CherryHitbox>().TakeDamage(1, null);
    }

    public override void OnAdded(GameObject cherry)
    {
        // Add VFX to cherry

        // Set cherry field to the GameObject cherry argument
        this.cherry = cherry;
        this.damageMultiplier = 2;
    }

    public override void OnRemoved(GameObject cherry)
    {
        // Remove VFX from cherry

        // Should not error when object is null
        throw new System.NotImplementedException();
    }
}
