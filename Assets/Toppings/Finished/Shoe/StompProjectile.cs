using UnityEngine;

public class StompProjectile : Projectile
{
    Vector3 target;
    [SerializeField] float startHeight = 2;
    [SerializeField] float fallSpeed = 12;
    float timer = 0;
    [SerializeField] float delayTime = 0.2f;
    bool reachedGoal = false;
    [SerializeField] float stopHeightOffset = default;
    void Start()
    {
        TrackFunctions.PointID pointID = TrackFunctions.trackFunctions.GetClosestPointOnTrack(target);
        Vector3 rotation = Vector3.RotateTowards(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(pointID.position.x, 0, pointID.position.z), 100, 0);
        transform.SetPositionAndRotation(target + new Vector3(0, startHeight, 0), Quaternion.LookRotation(rotation));
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < delayTime)
        {
            transform.position += (fallSpeed / 3) * Time.deltaTime * Vector3.up;
        }
        else if (!reachedGoal)
        {
            if (transform.position.y < target.y + stopHeightOffset)
            {
                reachedGoal = true;
                Destroy(gameObject, 0.1f);
            }
            transform.position += fallSpeed * Time.deltaTime * Vector3.down;
        }
    }

    public override void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public override Vector3 GetAttackDirection(GameObject attackedObject)
    {
        return Vector3.up;
    }
}
