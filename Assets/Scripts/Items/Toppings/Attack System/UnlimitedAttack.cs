using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A DirectAttack that attacks all Cherries in its target radius at once.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Unlimited Attack")]
public class UnlimitedAttack : DirectAttack
{
    public override void OnStart() {
        Debug.Log("Unlimited attack with a cooldown of " + this.cooldown + " seconds assigned to topping " + this.toppingFirePointObj.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        //
    }

    public override void OnCycle(GameObject targetedCherry) {
        //AttackAllCherries(toppingObj.GetComponent<TargetingSystem>().GetVisibleCherries());
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
