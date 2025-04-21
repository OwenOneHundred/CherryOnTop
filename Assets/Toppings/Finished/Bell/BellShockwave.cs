using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/BellShockwave")]
public class BellShockwave : ShockwaveAttack
{
    float baseCooldown = 0;
    [SerializeField] float scaleAmountPerItem = 0.05f;
    [SerializeField] protected float cameraShakeLength = 0.1f;
    [SerializeField] AudioFile onScale;
    ToppingActivatedGlow toppingActivatedGlow;
    public override void OnStart()
    {
        baseCooldown = cooldown;
    }
    public override void EveryFrame()
    {
        if (toppingActivatedGlow == null) { toppingActivatedGlow = toppingObj.transform.root.GetComponentInChildren<ToppingActivatedGlow>();}
        float newCooldown = Mathf.Clamp(baseCooldown - (scaleAmountPerItem * Inventory.inventory.GetInventoryCount(true)), 1f, baseCooldown);
        if (newCooldown < cooldown)
        {
            SoundEffectManager.sfxmanager.PlayOneShot(onScale);
            toppingActivatedGlow.StartNewFireEffect("Gold", Color.yellow, 2.5f);
        }
        cooldown = newCooldown;
    }

    public override void OnCycle(GameObject targetedCherry)
    {
        base.OnCycle(targetedCherry);
        Camera.main.transform.parent.GetComponent<CameraControl>().ApplyCameraShake(cameraShakeLength, 0.15f);
    }
}
