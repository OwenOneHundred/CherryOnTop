using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/XylophoneAttack")]
public class XylophoneAttack : SimpleAttack
{
    float baseCooldown = 0;
    [SerializeField] float scaleAmountPerItem = 0.1f;
    int lastFrameOwnedItems = 0;
    float pitchBend = -0.5f;
    readonly int pitchBendCount = 6;
    int pitchBendCounter = 0;
    public override void OnStart()
    {
        baseCooldown = cooldown;
    }

    public override void EveryFrame()
    {
        int count = Inventory.inventory.GetInventoryCount(false);
        if (count != lastFrameOwnedItems)
        {
            cooldown = Mathf.Clamp(baseCooldown - (scaleAmountPerItem * Inventory.inventory.GetBiggestStack(true)), 0.05f, baseCooldown);
            lastFrameOwnedItems = count;
        }
    }

    public override GameObject SpawnProjectile(GameObject projectile, Vector3 position, Vector3 velocity, Quaternion rotation, int damage)
    {
        GameObject newProjectile = Instantiate(projectile, position, rotation);
        newProjectile.GetComponent<Rigidbody>().linearVelocity = velocity;
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        projectileScript.damage = damage;
        projectileScript.owner = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;

        // Destroy the projectile after 8 seconds in case it misses the target
        Destroy(newProjectile, 8);

        pitchBendCounter += 1;
        if (pitchBendCounter < pitchBendCount)
        {
            pitchBend += 0.2f;
        }
        else
        {
            pitchBend = -0.5f;
            pitchBendCounter = 0;
        }
        SoundEffectManager.sfxmanager.PlayOneShotWithPitch(fireSound, 1 + pitchBend);

        CustomProjectileActions(newProjectile);

        return newProjectile;
    }
}