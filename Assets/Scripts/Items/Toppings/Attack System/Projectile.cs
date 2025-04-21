using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{   
    // Base damage that a projectile deals onto a cherry
    [System.NonSerialized] public int damage = 20;
    [System.NonSerialized] public Topping owner;
    [SerializeField] List<CherryDebuff> cherryDebuffs;
    [System.NonSerialized] public Rigidbody rb;
    [SerializeField] int maxHits = 5;
    int hitCount = 0;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetDamage(damage);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (hitCount >= maxHits) { return; }
        if (other.transform.root.TryGetComponent<CherryHitbox>(out CherryHitbox ch))
        {
            OnHitCherry(ch);
            float remainingCherryHealth = ch.TakeDamage(damage, owner, GetAttackDirection(other.gameObject));
            
            if (owner != null)
            {
                owner.OnHitCherry(ch);
            }
            
            if (remainingCherryHealth <= 0) { owner.OnKillCherry(ch); }

            foreach (CherryDebuff originalDebuff in cherryDebuffs)
            {
                other.transform.root.GetComponentInChildren<DebuffManager>().AddDebuff(originalDebuff);
            }

            hitCount += 1;
            if (hitCount >= maxHits)
            {
                SelfDestruct();
            }
        }
    }

    public virtual void OnHitCherry(CherryHitbox ch) {
        //
    }

    public virtual Vector3 GetAttackDirection(GameObject attackedObject)
    {
        return rb.linearVelocity;
    }

    public virtual void SelfDestruct()
    {
        Destroy(gameObject);
    }

    public virtual void SetTarget(Vector3 target)
    {

    }
}
