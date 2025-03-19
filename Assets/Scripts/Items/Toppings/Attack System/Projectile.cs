using System.Collections.Generic;
using UnityEngine;


public class Projectile : MonoBehaviour
{   
    // Base damage that a projectile deals onto a cherry
    public int damage = 20;
    public Topping owner; // TODO this is never set, so it's always null. Set this when fired.
    [SerializeField] List<CherryDebuff> cherryDebuffs;

    public void Start()
    {
        SetDamage(damage);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.TryGetComponent<CherryHitbox>(out CherryHitbox ch))
        {
            ch.TakeDamage(damage, owner);
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
}
