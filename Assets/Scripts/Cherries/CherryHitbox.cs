using UnityEngine;

/// <summary>
/// Registers hits from projectiles, reduces health, dies when at zero
/// </summary>
public class CherryHitbox : MonoBehaviour
{
    public float cherryHealth = 100;
    public float damage = 5;
    DebuffManager debuffManager;

    public void Start()
    {
        
    }

    public void Update()
    {   
        if (cherryHealth == 0) {
            // delete cherry
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // checking if "other" is a projectile(?)
        if (other)
        {
            cherryHealth -= debuffManager.GetDamageMultiplier() * damage;
        }
    }

}
