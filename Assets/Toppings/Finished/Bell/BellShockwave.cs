using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/BellShockwave")]
public class BellShockwave : ShockwaveAttack
{
    float baseCooldown = 0;
    [SerializeField] float scaleAmountPerItem = 0.05f;
    [SerializeField] protected float cameraShakeLength = 0.1f;
    public override void OnStart()
    {
        baseCooldown = cooldown;
    }
    public override void EveryFrame()
    {
        cooldown = Mathf.Clamp(baseCooldown - (scaleAmountPerItem * Inventory.inventory.ownedItems.Count), 1f, baseCooldown);
    }

    public override void OnCycle(GameObject targetedCherry)
    {
        base.OnCycle(targetedCherry);
        Camera.main.transform.parent.GetComponent<CameraControl>().ApplyCameraShake(cameraShakeLength, 0.15f);
    }
}
