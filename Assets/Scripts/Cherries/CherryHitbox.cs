using UnityEngine;

/// <summary>
/// Registers hits from projectiles, reduces health, dies when at zero
/// </summary>
public class CherryHitbox : MonoBehaviour
{
    public float cherryHealth;
    DebuffManager debuffManager;
    [SerializeField] AudioFile deathSound;
    [SerializeField] GameObject onDamagedPS;
    bool dead = false;

    public void Start()
    {
        debuffManager = GetComponent<DebuffManager>();
    }

    public void TakeDamage(float damage, Topping attacker, Vector3 directionOfDamage = default)
    {
        if (dead) { return; }
        
        float actualDamage = debuffManager.GetDamageMultiplier(attacker) * damage;
        cherryHealth -= actualDamage;
        if (directionOfDamage != default)
        {
            GameObject newOnDamagedPS = Instantiate(onDamagedPS, transform.position, Quaternion.identity);
            newOnDamagedPS.transform.rotation = Quaternion.LookRotation(directionOfDamage);
        }

        if (cherryHealth <= 0)
        {
            Die();
        }

        debuffManager.OnDamaged(damage);
    }

    private void Die()
    {
        dead = true;
        CherryManager.Instance.OnCherryKilled(GetComponent<CherryMovement>());
        SoundEffectManager.sfxmanager.PlayOneShot(deathSound);
        Destroy(gameObject);
    }
}
