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
        // Checks if cherry is big or not, then set cherryHealth.
        // May need to edit based on how big cherries are implemented.
        if (gameObject.transform.localScale == new Vector3(2, 2, 2))
        {
            cherryHealth = 75;
        }
        else
        {
            cherryHealth = 50;
        }
    }

    public void Update()
    {   
        if (cherryHealth == 0) {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        cherryHealth -= debuffManager.GetDamageMultiplier() * damage;
    }

}
