using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Special/TackShooter")]
public class PineappleAttack : ProjectileAttack
{
    [SerializeField] int numberOfBullets = 8;
    [SerializeField] float bulletOffsetFromCenter = 0.5f;
    [SerializeField] float heightOffset = 0.4f;

    public override void OnCycle(GameObject targetedCherry)
    {
        for (int i = 0; i < numberOfBullets; i++)
        {
            int angle = (360 / numberOfBullets) * i;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 rotatedVector = rotation * Vector3.right;
            Vector3 speedVector = rotatedVector * projectileSpeed;
            SpawnProjectile(projectile,
                toppingFirePointObj.transform.position + (rotatedVector * bulletOffsetFromCenter) + new Vector3(0, heightOffset, 0),
                speedVector,
                Quaternion.Euler(rotatedVector),
                damage);
        }
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
    }

    public override void OnStart()
    {
    }
}
