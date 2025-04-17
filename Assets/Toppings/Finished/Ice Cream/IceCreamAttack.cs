using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/BarrageAttack")]
public class IceCreamAttack : ProjectileAttack
{
    [SerializeField] Vector3 baseOffset = new Vector3(0, 1.5f, 0);
    [SerializeField] Vector3 randomOffsetRadius = new Vector3(0.75f, 0, 0.75f);
    [SerializeField] float speed;
    public override void OnCycle(GameObject targetedCherry)
    {
        Vector3 position = targetedCherry.transform.position + baseOffset + new Vector3(
            UnityEngine.Random.Range(-randomOffsetRadius.x, randomOffsetRadius.x),
            UnityEngine.Random.Range(-randomOffsetRadius.y, randomOffsetRadius.y),
            UnityEngine.Random.Range(-randomOffsetRadius.z, randomOffsetRadius.z));
        SpawnProjectile(projectile, position, (targetedCherry.transform.position - position).normalized * speed, Quaternion.identity, damage);
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public override void OnStart()
    {
        
    }
}
