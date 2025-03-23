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
    float speed;

    // Represents the maximum radius this shockwave can have
    [SerializeField]
    float range;

    public override void OnStart() {
        Debug.Log("Shockwave attack with a cooldown of " + this.cooldown + " assigned to topping " + this.toppingObj.name + ".");
        if (speed == 0) {
            Debug.Log("Shockwave speed cannot be 0. Setting the speed to 1 by default.");
            speed = 1;
        }
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {
        Debug.Log("New Cherry Targeted");
    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
    }

    private void AttackCherry(GameObject targetedCherry) {
        GameObject newShockwave = Instantiate(this.shockwave, toppingObj.transform.position, Quaternion.identity);
        newShockwave.GetComponent<Shockwave>().damage = damage;
        newShockwave.GetComponent<Shockwave>().range = range;

        float duration = range / speed;
        Destroy(newShockwave, duration);
    }
}
