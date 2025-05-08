using UnityEngine;
using EventBus;

/// <summary>
/// Registers hits from projectiles, reduces health, dies when at zero
/// </summary>
public class CherryHitbox : MonoBehaviour
{
    public float cherryHealth;
    DebuffManager debuffManager;
    [SerializeField] GameObject onDamagedPS;
    [SerializeField] GameObject damageNumberPrefab;
    [SerializeField] bool spawnDamageNumbers = false;
    [SerializeField] GameObject deathAnimation;
    protected bool dead = false;

    public void Awake()
    {
        debuffManager = GetComponent<DebuffManager>();
    }

    public float TakeDamage(float damage, Topping attacker, Vector3 directionOfDamage = default)
    {
        if (dead) { return 0; }
        
        float actualDamage = debuffManager.GetDamageMultiplier(attacker) * damage;
        cherryHealth -= actualDamage;
        if (directionOfDamage != default)
        {
            GameObject newOnDamagedPS = Instantiate(onDamagedPS, transform.position, Quaternion.identity);
            Destroy(newOnDamagedPS, 4);
            newOnDamagedPS.transform.rotation = Quaternion.LookRotation(directionOfDamage);
        }

        if (cherryHealth <= 0)
        {
            Die();
        }

        if (spawnDamageNumbers) { SpawnDamageNumbers(Mathf.FloorToInt(damage)); }

        debuffManager.OnDamaged(damage);
        OnTakeDamage();

        return cherryHealth;
    }

    public virtual void OnTakeDamage()
    {
        // should be overridden by child classes
    }

    private void SpawnDamageNumbers(int damage)
    {
        GameObject newNumber = Instantiate(damageNumberPrefab);
        newNumber.GetComponent<DamageNumber>().SetDisplay(damage);
        newNumber.transform.position = transform.position + new Vector3 (0, 1, 0);
    }

    protected virtual void Die()
    {
        dead = true;
        CherryManager.Instance.OnCherryKilled(GetComponent<CherryMovement>());
        EventBus<CherryDiesEvent>.Raise(new CherryDiesEvent(gameObject));
        Instantiate(deathAnimation, transform.position, Quaternion.identity, null).transform.localScale = transform.localScale;
        Destroy(gameObject);
    }
}
