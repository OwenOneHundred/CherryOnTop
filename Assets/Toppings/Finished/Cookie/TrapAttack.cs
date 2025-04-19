using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Trap")]
public class TrapAttack : ToppingAttack
{
    [SerializeField] protected AudioFile fireSound;
    [SerializeField] protected float lifetime;
    [SerializeField] protected int maxTraps;
    [SerializeField] protected GameObject trapPrefab;
    [SerializeField] protected float range = 0; // THIS IS BAD! All range amounts should be on Projectile.cs.
    // Putting them on the targeting system means everything must use a targeting system, which is not necessary.
    List<TrackFunctions.LineSegment3D> lineSegments = new();
    protected int activeTraps;
    protected float timer = 0;
    [SerializeField] protected bool instaFireBetweenRounds = true;
    public virtual GameObject SpawnTrap(GameObject projectile, Vector3 goal, int damage, float lifetime)
    {
        if (activeTraps >= maxTraps) { return null; }

        GameObject newProjectile = Instantiate(projectile, toppingObj.transform.position, Quaternion.identity);
        TrackTrap trackTrapScript = newProjectile.GetComponent<TrackTrap>();
        trackTrapScript.damage = damage;
        trackTrapScript.owner = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;
        trackTrapScript.SetTarget(goal);
        trackTrapScript.trapAttack = this;

        // Destroy the projectile after 8 seconds in case it misses the target
        Destroy(newProjectile, lifetime);

        if (fireSound != null && fireSound.clip != null) { SoundEffectManager.sfxmanager.PlayOneShot(fireSound);}

        activeTraps += 1;

        return newProjectile;
    }

    public override void OnStart()
    {
        SetLineSegments(toppingObj.transform.position, range);
    }

    public override void EveryFrame()
    {
        timer += Time.deltaTime;
        if (GetCanInstaFire() || timer > cooldown)
        {
            SpawnTrap(trapPrefab, GetGoalPosition(), damage, lifetime);
            timer = 0;
        }
    }

    protected bool GetCanInstaFire()
    {
        return instaFireBetweenRounds && (RoundManager.roundManager.roundState == RoundManager.RoundState.shop);
    }

    public void SetLineSegments(Vector3 toppingPosition, float radius)
    {
        lineSegments = TrackFunctions.trackFunctions.GetAllLineSegmentsThatIntersectSphere(toppingPosition, radius);
    }

    /// <summary>
    /// Returns a random Vector3 corresponding to a point on the track within range.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetGoalPosition()
    {
        float totalLength = 0;
        float[] newScaledLengths = new float[lineSegments.Count];
        List<TrackFunctions.LineSegment3D> newLineSegments = new();

        for (int i = 0; i < lineSegments.Count; i++) {
            newLineSegments.Add(FindNewSegment(lineSegments[i], toppingObj.transform.position, range));
            totalLength += newLineSegments[i].length;
        }
        for (int i = 0; i < lineSegments.Count; i++) {
            newScaledLengths[i] = newLineSegments[i].length / totalLength;
        }

        float randomReal = Random.value; // random value from [0, 1]
        int lsIndex = 0;
        do {
            randomReal -= newScaledLengths[lsIndex];
            lsIndex++;
        } while (randomReal > 0);

        return FindPositionOnLine(newLineSegments[lsIndex - 1], randomReal, totalLength);
    }

    private Vector3 FindPositionOnLine(TrackFunctions.LineSegment3D ls, float remainder, float scale)
    {
        // remainder will always be <= 0 at this point. Start at end of segment and work backwards
        Vector3 lsDirection = (ls.pointB - ls.pointA).normalized;
        Vector3 position = ls.pointB + remainder * scale * lsDirection;
        return position;

    }

    private TrackFunctions.LineSegment3D FindNewSegment(TrackFunctions.LineSegment3D ls, Vector3 center, float radius)
    {
        bool startInRange = (ls.pointA - center).magnitude <= radius;
        bool endInRange = (ls.pointB - center).magnitude <= radius;

        Vector3 newStart = ls.pointA;
        Vector3 newEnd = ls.pointB;
        Vector3 lsDirection = (ls.pointB - ls.pointA).normalized;

        Debug.Log("Radius: " + radius);
        if (!startInRange)
        {
            TrackFunctions.LineSegment3D rls = TrackFunctions.GetSimplifiedLineSegment3D(center, ls);
            Vector3 relVect = rls.pointA;
            
            float cutLength = -1 * relVect.x - Mathf.Sqrt(radius * radius - relVect.y * relVect.y);

            newStart = ls.pointA + cutLength * lsDirection;
        }
        if (!endInRange)
        {
            TrackFunctions.LineSegment3D rls = TrackFunctions.GetSimplifiedLineSegment3D(center, ls);
            Vector3 relVect = rls.pointB;
            
            float cutLength = -1 * relVect.x + Mathf.Sqrt(radius * radius - relVect.y * relVect.y);

            newEnd = ls.pointB + cutLength * lsDirection;
        }

        return new TrackFunctions.LineSegment3D(newStart, newEnd);
    }

    public override void OnCycle(GameObject targetedCherry)
    {
        
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public void TrapDestroyed()
    {
        activeTraps -= 1;
    }

}
