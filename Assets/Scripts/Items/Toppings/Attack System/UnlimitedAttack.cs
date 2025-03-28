using UnityEngine;

/// <summary>
/// A DirectAttack that attacks all Cherries in its target radius at once.
/// </summary>
public class UnlimitedAttack : DirectAttack
{
    [SerializeField] List<CherryDebuff> debuffs;

    public override void Start() {
        Debug.Log("Unlimited attack with a cooldown of " + this.cooldown + " seconds assigned to topping " + this.toppingObj.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        //
    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackAllCherries(toppingObj.GetComponent<TargetingSystem>().GetVisibleCherries());
    }

    public override void DealDamage(GameObject targetedCherry) {
        targetedCherry.GetComponentInParent<CherryHitbox>().TakeDamage(
            this.damage,
            toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping,
            targetedCherry.transform.position - toppingObj.transform.position);
        targetedCherry.GetComponentInParent<DebuffManager>().AddDebuffs(debuffs);
    }

    /// <summary>
    /// Attacks all cherries in range and in view.
    /// </summary>
    /// <param name="cherries"></param>
    private void AttackAllCherries(List<Collider> cherries) {
        foreach (Collider cherry in cherries) {
            DealDamage(cherry.gameObject);
        }
    }
}
