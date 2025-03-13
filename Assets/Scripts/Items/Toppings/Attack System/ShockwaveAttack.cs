using UnityEngine;

/// <summary>
/// A ToppingAttack that shoots a shockwave in all directions if a Cherry is in range.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Shockwave Attack")]
public class ShockwaveAttack : ToppingAttack
{
    // Stores a reference to the prefab used as the shockwave
    [SerializeField]
    GameObject shockwave;

    // Represents the speed of the shockwave produced by this attack, in radius units per second
    [SerializeField]
    float shockwaveSpeed;

    // Represents the maximum radius this shockwave can have
    [SerializeField]
    float shockwaveReach;

    public override void OnStart() {
        Debug.Log("Simple attack with a cooldown of " + this.cooldown + " assigned to topping " + this.topping.name + ".");
        if (shockwaveSpeed == 0) {
            Debug.Log("Shockwave speed cannot be 0. Setting the speed to 1 by default.");
            shockwaveSpeed = 1;
        }
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

        float shockwaveDuration = shockwaveReach / shockwaveSpeed;
        Destroy(newShockwave, shockwaveDuration);
    }
}
