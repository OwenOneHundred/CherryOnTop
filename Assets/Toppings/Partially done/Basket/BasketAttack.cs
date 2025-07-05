using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/Special/BasketAttack")]
public class BasketAttack : ArtilleryAttack
{
    AttackManager attackManager;
    float baseAttackCooldown = 1;
    ToppingObjectScript toppingObjectScript;
    [SerializeField] float reductionAmount;
    public override void OnStart()
    {
        attackManager = toppingFirePointObj.GetComponent<AttackManager>();
        toppingObjectScript = toppingFirePointObj.transform.root.GetComponent<ToppingObjectScript>();
        baseAttackCooldown = attackManager.GetAttackCooldown();
    }

    public override void EveryFrame()
    {
        float newCooldown = baseAttackCooldown * Mathf.Pow(1 - reductionAmount, toppingObjectScript.topping.TriggersCount);
        attackManager.SetAttackCooldown(newCooldown < 0.001f ? 0.001f : newCooldown);
    }
}
