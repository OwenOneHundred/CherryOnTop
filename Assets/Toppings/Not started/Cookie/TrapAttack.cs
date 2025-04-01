using System.Collections.Generic;
using UnityEngine;

public class TrapAttack : ToppingAttack
{
    [SerializeField] protected AudioFile fireSound;
    [SerializeField] protected float lifetime;
    [SerializeField] protected int maxTraps;
    [SerializeField] protected GameObject trapPrefab;
    protected int activeTraps;
    protected float timer = 0;
    public virtual GameObject SpawnTrap(GameObject projectile, Vector3 goal, int damage, float lifetime)
    {
        if (activeTraps >= maxTraps) { return null; }

        GameObject newProjectile = Instantiate(projectile, toppingObj.transform.position, Quaternion.identity);
        Projectile projectileScript = newProjectile.GetComponent<Projectile>();
        projectileScript.damage = damage;
        projectileScript.owner = toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;

        // Destroy the projectile after 8 seconds in case it misses the target
        Destroy(newProjectile, lifetime);

        if (fireSound != null && fireSound.clip != null) { SoundEffectManager.sfxmanager.PlayOneShot(fireSound);}

        activeTraps += 1;

        return newProjectile;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > cooldown)
        {
            Vector3 goalPosition = GetGoalPosition(toppingObj.transform.position);
            SpawnTrap(trapPrefab, goalPosition, damage, lifetime);
            timer = 0;
        }
    }

    public virtual Vector3 GetGoalPosition(Vector3 toppingPosition)
    {
        //List<Vector3> positions = TrackFunctions.trackFunctions.GetClosestPointsOnTrack(toppingObj.transform.position, 5);
        return Vector3.zero;
    }

    public override void OnCycle(GameObject targetedCherry)
    {
        
    }

    public override void OnNewCherryFound(GameObject newTargetedCherry)
    {
        
    }

    public override void OnStart()
    {
        
    }
}
