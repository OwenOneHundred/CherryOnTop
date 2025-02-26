using UnityEngine;

/// <summary>
/// A ToppingAttack that shoots a shockwave in all directions if a Cherry is in range.
/// </summary>
public class ShockwaveAttack : ToppingAttack
{
    // Stores a reference to the prefab used as the shockwave
    [SerializeField]
    GameObject shockwave;

    // Represents the speed of the shockwave produced by this attack
    [SerializeField]
    float shockwaveSpeed;

    public override void OnStart() {
        Debug.Log("Simple attack with a cooldown of " + this.cooldown + " assigned to topping " + this.topping.name + ".");
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        Debug.Log("New Cherry Targeted");
    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
    }

    private void AttackCherry(GameObject targetedCherry) {
        GameObject newShockwave = Instantiate(this.shockwave, topping.transform.position, Quaternion.identity);
        newShockwave.GetComponent<ShockwaveBehaviour>().shockwaveSpeed = this.shockwaveSpeed;

        Destroy(newShockwave, 8);
    }
}
