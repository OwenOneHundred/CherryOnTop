using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Special/VaultAttack")]
public class VaultAttack : SimpleAttack
{
    AttackManager attackManager;
    float baseAttackCooldown = -9999;
    public float reductionAmount = 0.05f;
    public override void EveryFrame()
    {
        if (baseAttackCooldown == -9999)
        {
            attackManager = toppingFirePointObj.transform.root.GetComponentInChildren<AttackManager>();
            baseAttackCooldown = attackManager.GetAttackCooldown();
        }
        
        float newCooldown = baseAttackCooldown * Mathf.Pow(1 - reductionAmount, Item.highestSellPrice);
        attackManager.SetAttackCooldown(newCooldown < 0.001f ? 0.001f : newCooldown);
    }
}
