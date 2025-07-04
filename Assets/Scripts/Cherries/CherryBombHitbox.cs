using UnityEngine;
using System.Collections;

public class CherryBombHitbox : CherryHitbox
{
    private float explosionTimer;
    [System.NonSerialized] public bool damaged;

    [SerializeField]
    private float explosionTime;

    [SerializeField]
    private float explosionRadius;

    [SerializeField]
    private float explosionStunTime;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject shockwave;
    [SerializeField] ParticleSystem fusePS;

    public void Start()
    {
        damaged = false;
        explosionTimer = 0;
    }

    public void Update()
    {
        if (damaged)
        {
            explosionTimer += Time.deltaTime;
            if (explosionTimer >= explosionTime)
            {
                Explode();
            }
        }
    }

    public override void OnTakeDamage()
    {
        if (damaged == true) { return; }
        damaged = true;
        fusePS.Play();
    }

    public void Explode()
    {
        Collider[] toppingsInRange = Physics.OverlapSphere(transform.position, explosionRadius, layerMask);

        GameObject newShockwave = Instantiate(shockwave, transform.position, Quaternion.identity);
        newShockwave.GetComponent<Shockwave>().range = explosionRadius;

        foreach(Collider toppingCollider in toppingsInRange)
        {
            AttackManager attackManager = toppingCollider.transform.root.GetComponentInChildren<AttackManager>();
            if (attackManager != null)
            {
                attackManager.Stun(explosionStunTime);
            }
        }

        Die();
    }
}
