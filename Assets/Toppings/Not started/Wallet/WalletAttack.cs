using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Special/Wallet")]
public class WalletAttack : SimpleAttack
{
    AttackManager attackManager;
    float baseAttackCooldown = -9999;
    float reductionAmountPercentage = 1;
    public override void EveryFrame()
    {
        if (baseAttackCooldown != -9999)
        {
            attackManager = toppingObj.transform.root.GetComponentInChildren<AttackManager>();
            baseAttackCooldown = attackManager.GetAttackCooldown();
        }
        float newCooldown = (baseAttackCooldown - (baseAttackCooldown * (100 / (Inventory.inventory.Money * reductionAmountPercentage))));
        attackManager.SetAttackCooldown(newCooldown < 0.01f ? 0.01f : newCooldown);
    }
}
