using UnityEngine;

/// <summary>
/// A ToppingAttack that shoots a shockwave in all directions if a Cherry is in range.
/// </summary>
[CreateAssetMenu(menuName = "Attacks/Shockwave Attack")]
public class ShockwaveAttack : ProjectileAttack
{
    // Stores a reference to the prefab used as the shockwave
    [SerializeField] GameObject shockwave;

    // Represents the speed of the shockwave produced by this attack, in radius units per second
    [SerializeField]
    float speed;

    // Represents the maximum radius this shockwave can have
    [SerializeField]
    float range;

    public override void OnStart() {
        
        if (speed == 0) {
            Debug.Log("Shockwave speed cannot be 0. Setting the speed to 1 by default.");
            speed = 1;
        }
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry) {

    }

    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
        if (fireSound.clip != null)
        {
            SoundEffectManager.sfxmanager.PlayOneShot(fireSound);
        }
    }

    public override GameObject SpawnProjectile(GameObject projectile, Vector3 position, Vector3 velocity, Quaternion rotation, int damage) {
        GameObject newShockwave = Instantiate(this.shockwave, toppingFirePointObj.transform.position, Quaternion.identity);
        newShockwave.GetComponent<Shockwave>().damage = damage;
        newShockwave.GetComponent<Shockwave>().range = range;
        newShockwave.GetComponent<Shockwave>().speed = speed;
        newShockwave.GetComponent<Shockwave>().owner = toppingFirePointObj.transform.root.GetComponent<ToppingObjectScript>().topping;

        float duration = range / speed;
        Destroy(newShockwave, duration);

        return newShockwave;
    }

    private void AttackCherry(GameObject targetedCherry) {
        SpawnProjectile(this.shockwave, toppingFirePointObj.transform.position, Vector3.zero, Quaternion.identity, this.damage);
    }

    public override string GetPierce()
    {
        return shockwave.GetComponent<Projectile>().maxHits + "";
    }
}
