using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{   
    // Base damage that a projectile deals onto a cherry
    [System.NonSerialized] public int damage = 20;
    public Topping owner; // TODO this is never set, so it's always null. Set this when fired.
    [SerializeField] List<CherryDebuff> cherryDebuffs;
    Rigidbody rb;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetDamage(damage);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<CherryHitbox>(out CherryHitbox ch))
        {
            ch.TakeDamage(damage, owner, GetAttackDirection(other.gameObject));
            OnHitCherry(ch);

            foreach (CherryDebuff originalDebuff in cherryDebuffs)
            {
                CherryDebuff debuffCopy = Instantiate(originalDebuff);
                other.GetComponentInChildren<DebuffManager>().AddDebuff(debuffCopy);
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
}
