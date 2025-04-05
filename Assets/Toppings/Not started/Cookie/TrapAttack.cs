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
    List<TrackFunctions.LineSegment> nearEnoughLineSegments = new();
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
        List<TrackFunctions.LineSegment> lineSegments = TrackFunctions.trackFunctions.GetAllLineSegmentsThatIntersectCircle(toppingPosition, radius);
    }

    private Vector3 GetGoalPosition()
    {
        return Vector3.zero; // TODO
    }

    public override void OnCycle(GameObject targetedCherry)
    {
        
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }
}
