using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Special/Wallet")]
public class WalletAttack : SimpleAttack
{
    AttackManager attackManager;
    float baseAttackCooldown = -9999;
    public float reductionAmount = 0.5f;
    public override void EveryFrame()
    {
        if (baseAttackCooldown == -9999)
        {
            attackManager = toppingObj.transform.root.GetComponentInChildren<AttackManager>();
            baseAttackCooldown = attackManager.GetAttackCooldown();
        }
        float newCooldown = baseAttackCooldown * Mathf.Pow(1 - reductionAmount, Inventory.inventory.Money);
        attackManager.SetAttackCooldown(newCooldown < 0.001f ? 0.001f : newCooldown);
    }
}
