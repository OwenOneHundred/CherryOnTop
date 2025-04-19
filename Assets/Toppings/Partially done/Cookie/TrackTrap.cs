using UnityEngine;

public class TrackTrap : ExplodingProjectile
{
    Vector3 target;
    [SerializeField] float slideSpeed = 5;
    bool atGoal = false;
    [System.NonSerialized] public TrapAttack trapAttack;

    void Update()
    {
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
        Debug.Log(target);
        this.target = target;
    }

    void OnDestroy()
    {
        trapAttack.TrapDestroyed();
    }
}
