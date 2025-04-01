using UnityEngine;

public class ArtilleryProjectile : ExplodingProjectile
{
    [SerializeField] float initialUpwardSpeed = 10;
    float upwardSpeed = 10;
    float horizontalSpeed = 10;
    [SerializeField] float flyTimeSeconds = 2;
    Vector3 target;
    bool reachedTop = false;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float rotationSpeed = 0;
    
    void Start()
    {
        if (target == Vector3.zero) { Explode(); }

        upwardSpeed = initialUpwardSpeed;

        horizontalSpeed = Vector2.Distance(new Vector2(rb.position.x, rb.position.z), new Vector2(target.x, target.z)) / flyTimeSeconds;
        if (rotationSpeed != 0)
        {
            rb.angularVelocity = new Vector3(rotationSpeed * Random.Range(-1f, 1f), rotationSpeed * Random.Range(-1f, 1f), rotationSpeed * Random.Range(-1f, 1f));
        }
    }

    void FixedUpdate()
    {
        if (rb.position.y <= target.y && reachedTop)
        {
            Explode();
            return;
        }

        reachedTop = upwardSpeed < 0;
        
        Vector2 xzMovement = (new Vector2(target.x, target.z) - new Vector2(transform.position.x, transform.position.z)).normalized * horizontalSpeed * Time.deltaTime;
        transform.position += new Vector3(xzMovement.x, upwardSpeed * Time.deltaTime, xzMovement.y);

        upwardSpeed -= Time.deltaTime * gravity; // this line is sus
    }

    public override void SetTarget(Vector3 targetPosition)
    {
        target = targetPosition;
    }
}
