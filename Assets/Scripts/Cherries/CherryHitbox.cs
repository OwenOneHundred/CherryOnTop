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

    public void TakeDamage(float damage)
    {
        float actualDamage = debuffManager.GetDamageMultiplier() * damage;
        cherryHealth -= actualDamage;

        if (cherryHealth <= 0)
        {
            CherryManager.Instance.OnCherryKilled(GetComponent<CherryMovement>());
            SoundEffectManager.sfxmanager.PlayOneShot(deathSound);
            Destroy(gameObject);
        }
    }
}
