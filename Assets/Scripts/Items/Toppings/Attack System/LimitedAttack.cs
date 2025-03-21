using UnityEngine;
using System.Collections;

/// <summary>
/// A DirectAttack that can only perform a limited number of attacks before stopping (idle)
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Limited Attack")]
public class LimitedAttack : DirectAttack
{
    // Represents the maximum number of attacks that can be performed before the topping becomes idle.
    [SerializeField]
    int attackLimit;

    // Represents the amount of time to wait before dealing damage.
    [SerializeField]
    float attackDelay;

    // Represents whether the Topping is free to attack (true) or is hindered by its cooldown (false)
    private bool onCooldown = false;

    // Represents the number of attacks that have been performed by this topping
    private int attacks;

    public override void OnStart() {
        Debug.Log("Limited attack with a cooldown of " + this.cooldown + " seconds assigned to topping " + this.topping.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        if (attacks < attackLimit) {
            // These if statements are nested in case we want to do something special when the topping runs out of attacks.
            if (!onCooldown) {
                this.topping.GetComponent<AttackManager>().StartCoroutine(DelayedConditionalAttack(newTargetedCherry, attackDelay));
                onCooldown = true;
            }
        } else {
            // Do something here?
        }
    }

    public override void OnCycle(GameObject targetedCherry) {
        onCooldown = false;
    }

    public override void DealDamage(GameObject targetedCherry) {
        targetedCherry.GetComponent<CherryHitbox>().TakeDamage(this.damage);
    }

    /// <summary>
    /// Attacks the targeted Cherry.
    /// </summary>
    /// <param name="targetedCherry"></param>
    private void AttackCherry(GameObject targetedCherry) {
        DealDamage(targetedCherry);
        attacks++;
    }

    /// <summary>
    /// Waits a specified number of seconds and then attacks the specified targeted Cherry, but only if it is still in range.
    /// </summary>
    /// <param name="targetedCherry"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator DelayedConditionalAttack(GameObject targetedCherry, float delay) {
        yield return new WaitForSeconds(delay);
        if (targetedCherry == this.topping.GetComponent<AttackManager>().GetTargetedCherry()) {
            AttackCherry(targetedCherry);
        }
    }
}
