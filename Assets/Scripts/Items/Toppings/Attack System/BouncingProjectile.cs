using System.Collections.Generic;
using UnityEngine;

public class BouncingProjectile : Projectile
{
    [SerializeField] float bounceSeekDistance = 3;
    [SerializeField] LayerMask cherryLayer;
    List<CherryHitbox> hitCherries = new List<CherryHitbox>();
    [SerializeField] float reboundVelocity = 10;
    [SerializeField] int maxBounces = 3;
    int bounces = 0;
    public override void OnHitCherry(CherryHitbox ch)
    {
        if (bounces >= maxBounces) { return; }

        hitCherries.Add(ch);

        CherryHitbox nextTarget = GetNextTarget();

        if (nextTarget == null) { return; }

        bounces += 1;

        SetVelocityTowardTarget(nextTarget);
    }

    private CherryHitbox GetNextTarget()
    {
        Collider[] colliders = new Collider[20];
        Physics.OverlapSphereNonAlloc(transform.position, bounceSeekDistance, colliders, cherryLayer);
        float closestDistance = bounceSeekDistance * 10;
        CherryHitbox closestHitbox = null;
        foreach (Collider collider in colliders)
        {
            if (collider == null) { break; }
            CherryHitbox cherryHitbox = collider.transform.root.GetComponent<CherryHitbox>();
            if (!hitCherries.Contains(cherryHitbox))
            {
                float distance = Vector3.Distance(cherryHitbox.transform.position, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestHitbox = cherryHitbox;
                }
            }
        }
        return closestHitbox;
    }

    private void SetVelocityTowardTarget(CherryHitbox target)
    {
        rb.linearVelocity = (target.transform.position - rb.position).normalized * reboundVelocity;
    }
}
