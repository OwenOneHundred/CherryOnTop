using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName ="Attacks/MouseAttack")]
public class MouseAttack : DirectAttack
{
    [SerializeField] GameObject visualEffectOnHit;
    [SerializeField] float effectLifetime = 2;
    [SerializeField] AudioFile sound;
    public override void OnCycle(GameObject targetedCherry)
    {
        DealDamage(targetedCherry);
        Vector3 towardCamera = Camera.main.transform.position - targetedCherry.transform.position;
        SoundEffectManager.sfxmanager.PlayOneShot(sound);
        Destroy(Instantiate(visualEffectOnHit, targetedCherry.transform.position + towardCamera.normalized, Quaternion.identity), effectLifetime);
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {

    }

    public override void OnStart()
    {

    }
}
