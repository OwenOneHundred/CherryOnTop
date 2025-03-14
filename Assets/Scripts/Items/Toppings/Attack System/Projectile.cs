using UnityEngine;


public class Projectile : MonoBehaviour
{   
    // Base damage that a projectile deals onto a cherry
    public float damage;

    public void Start()
    {
        SetDamage(damage);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<CherryHitbox>(out CherryHitbox ch))
        {
            ch.TakeDamage(damage);
        }
    }
}
