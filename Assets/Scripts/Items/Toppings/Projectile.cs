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
        Debug.Log("Trigger enter: " + other.transform.root.name);
        if (other.transform.root.TryGetComponent<CherryHitbox>(out CherryHitbox ch))
        {
            ch.TakeDamage(damage);
        }
    }
}
