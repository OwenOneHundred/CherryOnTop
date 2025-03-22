using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(menuName = "CherryDebuff/PoisonEffect")]
public class PoisonEffect : CherryDebuff
{
    private float timeSinceTick;
    private float effectDuration;

    public override void EveryFrame()
    {
        // Cherries take damage every tick
        this.timeSinceTick += Time.deltaTime;
        if (this.timeSinceTick >= this.effectDuration)
        {
            this.timeSinceTick = 0;
            this.cherry.GetComponent<CherryHitbox>().TakeDamage(1, null);
            this.effectDuration -= Time.deltaTime;
            Debug.Log("Poison damage");
        }
        if (this.effectDuration <= 0)
        {
            this.RemoveSelf();
        }

    }

    public override void OnAdded(GameObject cherry)
    {
        // Add VFX to cherry
        
        // Set cherry field to the GameObject cherry argument
        this.cherry = cherry;
    }

    public override void OnRemoved(GameObject cherry)
    {
        // Remove VFX from cherry

        // Should not error when object is null
        throw new System.NotImplementedException();
    }
}
