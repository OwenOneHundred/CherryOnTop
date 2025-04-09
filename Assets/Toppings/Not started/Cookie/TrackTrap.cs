using UnityEngine;

public class TrackTrap : Projectile
{
    Vector3 target;
    [SerializeField] float slideSpeed = 5;
    bool atGoal = false;

    void Update()
    {
        Debug.Log(atGoal);
        if (!atGoal && target != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, slideSpeed * Time.deltaTime);
            if (transform.position == target) 
            {
                atGoal = true;
            }
        }
    }

    public override Vector3 GetAttackDirection(GameObject attackedObject)
    {
        return Vector3.up;
    }

    public override void SetTarget(Vector3 target)
    {
        this.target = target;
    }
}
