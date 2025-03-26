using UnityEngine;

public class ArtilleryProjectile : Projectile
{
    [SerializeField] GameObject explosionObject;
    [SerializeField] float initialUpwardSpeed = 10;
    float upwardSpeed = 10;
    float horizontalSpeed = 10;
    [SerializeField] float flyTimeSeconds = 2;
    float targetY = 0;
    CherryMovement target;
    Vector2 directionToTarget;
    bool reachedTop = false;
    
    void Start()
    {
        target = CherryManager.Instance.GetHighestPriorityCherry();
        if (target == null) { Explode(); }

        upwardSpeed = initialUpwardSpeed;

        horizontalSpeed = Vector2.Distance(
                new Vector2(rb.position.x, rb.position.z),
                new Vector2(target.transform.position.x, target.transform.position.z)) / (flyTimeSeconds);
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            directionToTarget = (new Vector2(target.transform.position.x, target.transform.position.z) - new Vector2(rb.position.x, rb.position.z)).normalized;
            targetY = target.transform.position.y;
        }

        upwardSpeed -= Time.deltaTime * (initialUpwardSpeed / (flyTimeSeconds / 2));
        reachedTop = upwardSpeed < 0;
        Debug.Log("Direction to target: " + directionToTarget);
        
        rb.position += new Vector3(directionToTarget.x * horizontalSpeed * Time.fixedDeltaTime, upwardSpeed * Time.fixedDeltaTime, directionToTarget.y * horizontalSpeed * Time.fixedDeltaTime);

        if (rb.position.y < targetY && reachedTop)
        {
            Debug.Log("Didn't get anywhere");
            Explode();
        }
    }

    void Explode()
    {
        Destroy(gameObject);

        Debug.Log("Exploded");
    }
}
