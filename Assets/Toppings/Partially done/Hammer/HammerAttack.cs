using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/HammerAttack")]
public class HammerAttack : SimpleAttack
{
    float baseDamage = -9999;
    AttackManager attackManager;
    public override void EveryFrame()
    {
        if (baseDamage == -9999)
        {
            attackManager = toppingFirePointObj.transform.root.GetComponentInChildren<AttackManager>();
            baseDamage = attackManager.AttackDamage;
        }
        attackManager.AttackDamage = Mathf.FloorToInt(baseDamage + ToppingRegistry.toppingRegistry.PlacedToppings.Count);
    }

    public override void CustomProjectileActions(GameObject projectile)
    {
        projectile.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360));
    }
}
