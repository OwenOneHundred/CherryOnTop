using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/FlashAttack")]
public class CameraAttack : ToppingAttack
{
    TargetingSystem targetingSystem;
    [SerializeField] int pierce = 30;
    Topping topping;
    public override void OnCycle(GameObject targetedCherry)
    {
        Flash();
    }

    private void Flash()
    {
        Collider[] hits = Physics.OverlapSphere(toppingObj.transform.position, targetingSystem.GetRange());

        int totalHitCherries = 0;
        foreach (Collider hit in hits)
        {
            if (totalHitCherries > pierce) { return; }
            if (hit.transform.parent.TryGetComponent(out CherryHitbox cherryHitbox))
            {
                float remainingCherryHealth = cherryHitbox.TakeDamage(damage, topping, hit.transform.position - toppingObj.transform.position);
                foreach (CherryDebuff debuff in debuffs)
                {
                    cherryHitbox.GetComponent<DebuffManager>().AddDebuff(debuff);
                }

                if (topping != null)
                {
                    topping.OnHitCherry(cherryHitbox);
                }
                if (remainingCherryHealth <= 0) { topping.OnKillCherry(cherryHitbox); }

                totalHitCherries += 1;
            }
        }
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public override void OnStart()
    {
        targetingSystem = toppingObj.transform.root.GetComponentInChildren<TargetingSystem>();
        topping = toppingObj.GetComponent<ToppingObjectScript>().topping;
    }
}
