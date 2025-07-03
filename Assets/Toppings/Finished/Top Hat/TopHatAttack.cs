using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/RotatedAttack")]
public class TopHatAttack : SimpleAttack
{
    [SerializeField] GameObject wand;
    int attacks = 0;
    public override void OnCycle(GameObject targetedCherry) {
        AttackCherry(targetedCherry);
        attacks += 1;
        if (attacks >= 5)
        {
            attacks = 0;
            SpawnWand(targetedCherry);
        }
    }

    private void SpawnWand(GameObject cherry)
    {
        GameObject newWand = Instantiate(wand);
        newWand.GetComponent<MagicWand>().owner = toppingFirePointObj.transform.root.GetComponentInChildren<ToppingObjectScript>().topping;
        newWand.GetComponent<MagicWand>().SetTarget(cherry.transform.position);
    }

    private void AttackCherry(GameObject targetedCherry) {
        Vector3 direction = FindTargetVector(targetedCherry);
        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        Quaternion angle = Quaternion.RotateTowards(toppingFirePointObj.transform.rotation, targetRotation, 100);
        angle *= Quaternion.AngleAxis(90, Vector3.up);
        SpawnProjectile(this.projectile, toppingFirePointObj.transform.position, direction, angle, this.damage);
    }
}
