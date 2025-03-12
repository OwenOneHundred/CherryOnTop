using UnityEngine;

/// <summary>
/// Registers hits from projectiles, reduces health, dies when at zero
/// </summary>
public class CherryHitbox : MonoBehaviour
{
    public float cherryHealth = 100;
    DebuffManager debuffManager;

    public void Start()
    {
        debuffManager = GetComponent<DebuffManager>();
    }

    public void Update()
    {   
        if (cherryHealth <= 0) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        cherryHealth -= debuffManager.GetDamageMultiplier() * damage;
    }
}
