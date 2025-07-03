using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Special/GlassesAttack")]
public class GlassesAttack : ToppingAttack
{
    [SerializeField] GameObject laser;
    [SerializeField] AudioFile laserSound;
    public override void OnCycle(GameObject targetedCherry)
    {
        GameObject newLaser = Instantiate(laser);
        newLaser.GetComponent<GlassesLaserShrink>().SetUp(toppingFirePointObj, targetedCherry);

        CherryHitbox cherryHitbox = targetedCherry.transform.root.GetComponentInChildren<CherryHitbox>();
        Topping topping = toppingFirePointObj.transform.root.GetComponent<ToppingObjectScript>().topping;

        int mulitpliedDamage = damage * Mathf.RoundToInt(Mathf.Pow(5, cherryHitbox.GetComponent<DebuffManager>().GetDebuffCount()));
        cherryHitbox.TakeDamage(mulitpliedDamage,
            topping,
            (targetedCherry.transform.position - toppingFirePointObj.transform.position).normalized);

        SoundEffectManager.sfxmanager.PlayOneShot(laserSound);
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {

    }

    public override void OnStart()
    {
        
    }
}
