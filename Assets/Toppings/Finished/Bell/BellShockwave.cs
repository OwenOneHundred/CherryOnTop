using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/BellShockwave")]
public class BellShockwave : ShockwaveAttack
{
    float baseCooldown = 0;
    [SerializeField] float scaleAmountPerItem = 0.05f;
    [SerializeField] protected float cameraShakeLength = 0.1f;
    [SerializeField] AudioFile onScale;
    ToppingActivatedGlow toppingActivatedGlow;
    Topping topping;
    public override void OnStart()
    {
        baseCooldown = cooldown;
        topping = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;
    }
    public override void EveryFrame()
    {
        if (toppingActivatedGlow == null) { toppingActivatedGlow = toppingObj.transform.root.GetComponentInChildren<ToppingActivatedGlow>();}
        int itemCount = Inventory.inventory.GetInventoryCount(true);
        float newCooldown = Mathf.Clamp(baseCooldown - (scaleAmountPerItem * itemCount), 1f, baseCooldown);
        if (newCooldown < cooldown)
        {
            toppingActivatedGlow.StartNewFireEffect("Gold", Color.yellow, 2.5f);
        }
        cooldown = newCooldown;
        if (topping == null) { topping = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping; }
        topping.triggersCount = itemCount;
    }

    public override void OnCycle(GameObject targetedCherry)
    {
        base.OnCycle(targetedCherry);
        Camera.main.transform.parent.GetComponent<CameraControl>().ApplyCameraShake(cameraShakeLength, 0.15f);
    }
}
