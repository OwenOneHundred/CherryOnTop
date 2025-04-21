using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    // Represents whether the current targeted Cherry has been attacked at least once so far.
    private bool attackSuccessful = false;

    // Represents the number of attacks that have been performed by this topping
    private int attacks;

    public override void OnStart() {
        Debug.Log("Limited attack with a cooldown of " + this.cooldown + " seconds assigned to topping " + this.toppingObj.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        attackSuccessful = false;
    }

    public override void OnCycle(GameObject targetedCherry) {
        if (!attackSuccessful) {
            this.toppingObj.GetComponent<AttackManager>().StartCoroutine(DelayedConditionalAttack(targetedCherry, attackDelay));
            attackSuccessful = true;
        }
    }

    /// <summary>
    /// Attacks the targeted Cherry.
    /// </summary>
    /// <param name="targetedCherry"></param>
    private void AttackCherry(GameObject targetedCherry) {
        if (attacks < attackLimit)
        {
            DealDamage(targetedCherry);
            attacks++;
        }
    }

    /// <summary>
    /// Waits a specified number of seconds and then attacks the specified targeted Cherry, but only if it is still in range.
    /// </summary>
    /// <param name="targetedCherry"></param>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator DelayedConditionalAttack(GameObject targetedCherry, float delay) {
        yield return new WaitForSeconds(delay);
        if (targetedCherry != null && targetedCherry == this.toppingObj.GetComponent<AttackManager>().GetTargetedCherry()) {
            AttackCherry(targetedCherry);
            this.toppingObj.GetComponent<TargetingSystem>().AddTargetedCherry(targetedCherry);
        }
    }
}
