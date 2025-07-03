using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/HammerAttack")]
public class HammerAttack : SimpleAttack
{
    float baseDamage = 5;
    AttackManager attackManager;
    public override void OnStart()
    {
        attackManager = toppingFirePointObj.transform.root.GetComponentInChildren<AttackManager>();
        baseDamage = attackManager.AttackDamage;
    }
    public override void EveryFrame()
    {
        attackManager.AttackDamage = Mathf.FloorToInt(baseDamage + ToppingRegistry.toppingRegistry.PlacedToppings.Count);
    }

    public override void CustomProjectileActions(GameObject projectile)
    {
        projectile.transform.rotation = Quaternion.Euler(UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360), UnityEngine.Random.Range(-360, 360));
    }
}
