using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/BellShockwave")]
public class BellShockwave : ShockwaveAttack
{
    float baseCooldown = 0;
    [SerializeField] float scaleAmountPerItem = 0.05f;
    public override void OnStart()
    {
        baseCooldown = cooldown;
    }
    public override void EveryFrame()
    {
        cooldown = Mathf.Clamp(baseCooldown - (scaleAmountPerItem * Inventory.inventory.ownedItems.Count), 0.5f, cooldown);
    }
}
