using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Trap")]
public class TrapAttack : ToppingAttack
{
    [SerializeField] protected AudioFile fireSound;
    [SerializeField] protected float lifetime;
    [SerializeField] protected int maxTraps;
    [SerializeField] protected GameObject trapPrefab;
    [SerializeField] protected float range = 2; // THIS IS BAD! All range amounts should be on Projectile.cs.
    // Putting them on the targeting system means everything must use a targeting system, which is not necessary.
    List<TrackFunctions.LineSegment2D> lineSegments = new();
    protected int activeTraps;
    protected float timer = 0;
    public virtual GameObject SpawnTrap(GameObject projectile, Vector3 goal, int damage, float lifetime)
    {
        if (activeTraps >= maxTraps) { return null; }

        GameObject newProjectile = Instantiate(projectile, toppingObj.transform.position, Quaternion.identity);
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        projectileScript.damage = damage;
        projectileScript.owner = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;
        projectileScript.SetTarget(goal);

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

    System.Collections.IEnumerator Runner() // this is only necessary until we can refactor OnCycle or something
    {
        EveryFrame();
        yield return null;
    }

    void EveryFrame()
    {
        timer += Time.deltaTime;
        if (timer > cooldown)
        {
            SpawnTrap(trapPrefab, GetGoalPosition(), damage, lifetime);
            timer = 0;
        }
    }

    public void SetLineSegments(Vector3 toppingPosition, float radius)
    {
        lineSegments = TrackFunctions.trackFunctions.GetAllLineSegmentsThatIntersectCircle(toppingPosition, radius);
    }

    /// <summary>
    /// Returns a random Vector3 corresponding to a point on the track within range.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetGoalPosition()
    {
        float totalLength = 0;
        float[] newScaledLengths = new float[lineSegments.Count];
        List<TrackFunctions.LineSegment2D> newLineSegments = new();

        for (int i = 0; i < lineSegments.Count; i++) {
            newLineSegments[i] = FindNewSegment(lineSegments[i], new Vector2(toppingObj.transform.position.x, toppingObj.transform.position.z), range);
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

        return TrackFunctions.ToVector3D(FindPositionOnLine(newLineSegments[lsIndex - 1], randomReal, totalLength), toppingObj.transform.position.y);
    }

    private Vector2 FindPositionOnLine(TrackFunctions.LineSegment2D ls, float remainder, float scale)
    {
        // remainder will always be <= 0 at this point. Start at end of segment and work backwards
        Vector2 lsDirection = (ls.pointB - ls.pointA).normalized;
        Vector2 position = ls.pointB + remainder * scale * lsDirection;
        return position;

    }

    private TrackFunctions.LineSegment2D FindNewSegment(TrackFunctions.LineSegment2D ls, Vector2 center, float radius)
    {
        bool startInRange = (ls.pointA - center).magnitude <= radius;
        bool endInRange = (ls.pointB - center).magnitude <= radius;

        Vector2 newStart = ls.pointA;
        Vector2 newEnd = ls.pointB;
        Vector2 lsDirection = (ls.pointB - ls.pointA).normalized;

        if (!startInRange)
        {
            TrackFunctions.LineSegment2D rls = TrackFunctions.GetSimplifiedLineSegment(center, ls);
            Vector2 relVect = rls.pointA;
            
            float cutLength = -1 * relVect.x - Mathf.Sqrt(radius * radius - relVect.y * relVect.y);

            newStart = ls.pointA + cutLength * lsDirection;
        }
        if (!endInRange)
        {
            TrackFunctions.LineSegment2D rls = TrackFunctions.GetSimplifiedLineSegment(center, ls);
            Vector2 relVect = rls.pointB;
            
            float cutLength = -1 * relVect.x - Mathf.Sqrt(radius * radius - relVect.y * relVect.y);

            newEnd = ls.pointA + cutLength * lsDirection;
        }

        return new TrackFunctions.LineSegment2D(newStart, newEnd);
    }

    public override void OnCycle(GameObject targetedCherry)
    {
        
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }


}
