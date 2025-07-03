using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/FlashAttack")]
public class CameraAttack : ToppingAttack
{
    TargetingSystem targetingSystem;
    [SerializeField] int pierce = 30;
    Topping topping;
    [SerializeField] LayerMask cakeLayer;
    [SerializeField] AudioFile attackSound;
    public override void OnCycle(GameObject targetedCherry)
    {
        if (topping == null) { topping = toppingFirePointObj.transform.root.GetComponentInChildren<ToppingObjectScript>().topping; }
        Attack();
        PlayPS();
    }

    private void PlayPS()
    {
        toppingFirePointObj.transform.root.GetChild(2).GetComponent<ParticleSystem>().Play();
        SoundEffectManager.sfxmanager.PlayOneShot(attackSound);
    }

    private void Attack()
    {
        Collider[] hits = Physics.OverlapSphere(toppingFirePointObj.transform.position, targetingSystem.GetRange());

        int totalHitCherries = 0;
        foreach (Collider hit in hits)
        {
            if (totalHitCherries > pierce) { return; }
            if (hit.transform.root.TryGetComponent(out CherryHitbox cherryHitbox))
            {
                if (!HasClearLineOfSight(hit.transform)) { return; }
                float remainingCherryHealth = cherryHitbox.TakeDamage(damage, topping, hit.transform.position - toppingFirePointObj.transform.position);
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

    bool HasClearLineOfSight(Transform target)
    {
        Vector3 direction = (target.position - toppingFirePointObj.transform.position).normalized;
        float distance = Vector3.Distance(toppingFirePointObj.transform.position, target.position);
        RaycastHit hit;
        return !Physics.Raycast(toppingFirePointObj.transform.position, direction, out hit, distance, cakeLayer);
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public override void OnStart()
    {
        targetingSystem = toppingFirePointObj.transform.root.GetComponentInChildren<TargetingSystem>();
    }
}
