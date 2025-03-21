using UnityEngine;

/// <summary>
/// Registers hits from projectiles, reduces health, dies when at zero
/// </summary>
public class CherryHitbox : MonoBehaviour
{
    public float cherryHealth;
    DebuffManager debuffManager;
    [SerializeField] AudioFile deathSound;

    public void Start()
    {
        debuffManager = GetComponent<DebuffManager>();
    }

    public void TakeDamage(int damage, Topping attacker)
    {
        if (cherryHealth <= 0) { return; }
        
        float actualDamage = debuffManager.GetDamageMultiplier(attacker) * damage;
        cherryHealth -= actualDamage;

        if (cherryHealth <= 0)
        {
            Die();
        }

        debuffManager.OnDamaged(damage);
    }

    private void Die()
    {
        CherryManager.Instance.OnCherryKilled(GetComponent<CherryMovement>());
        SoundEffectManager.sfxmanager.PlayOneShot(deathSound);
        Destroy(gameObject);
    }
}
