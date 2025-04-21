using UnityEngine;

public class MagicWand : StompProjectile
{
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionRadius = 2;
    [SerializeField] float explosionSpeed = 4;
    public override void ReachedGoalPosition()
    {
        base.ReachedGoalPosition();

        GameObject newShockwave = Instantiate(explosion, transform.position, Quaternion.identity);
        Shockwave shockwave = newShockwave.GetComponent<Shockwave>();
        shockwave.damage = damage;
        shockwave.range = explosionRadius;
        shockwave.speed = explosionSpeed;
        shockwave.owner = owner;

        float duration = explosionRadius / explosionSpeed;
        Destroy(newShockwave, duration);
    }
}
