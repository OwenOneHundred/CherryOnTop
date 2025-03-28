using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    private LayerMask cherryLayer, cakeLayer;
    [SerializeField] AttackManager attackManager;

    protected GameObject currentCherry;
    private List<Collider> visibleCherries = new List<Collider>();
    private List<Collider> targetedCherries = new List<Collider>();

    void Start()
    {
        cherryLayer = LayerMask.GetMask("Cherry"); // Only detect enemies
        cakeLayer = LayerMask.GetMask("Cake"); // Walls
    }

    void Update()
    {
        GameObject found = Search();
        attackManager.UpdateTargetedCherry(found);
        currentCherry = found;
    }

    GameObject Search()
    {
        GameObject bestCherry = null;
        float highestDistance = -1f;

        List<Collider> cherries = Physics.OverlapSphere(transform.position, range, cherryLayer).ToList();
        //Check to see if cherries are only cherries not in targetedCherries
        cherries.RemoveAll(cherry => targetedCherries.Contains(cherry));

        foreach (Collider cherry in cherries)
        {
            CherryMovement cherryMovement = cherry.transform.root.GetComponent<CherryMovement>();
            if (HasClearLineOfSight(cherry.transform)) 
            {
                visibleCherries.Add(cherry);
            
                if (cherryMovement != null)
                {
                    if (cherryMovement.distanceTraveled > highestDistance)
                    {
                        highestDistance = cherryMovement.distanceTraveled;
                        bestCherry = cherry.gameObject;
                    }
                }
            }
        }

        return bestCherry;
    }


    bool HasClearLineOfSight(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit, distance, cakeLayer))
        {
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
            return false; // Blocked by Cake
        } else {

        Debug.DrawRay(transform.position, direction * distance, Color.yellow);
        return true; // Clear sight to Cherry
        }
    }

    public void AddTargetedCherry(GameObject cherry)
    {
        if (!targetedCherries.Contains(cherry.GetComponent<Collider>()))
        {
            targetedCherries.Add(cherry.GetComponent<Collider>());
        }
    }

    public List<Collider> GetTargetedCherries()
    {
        return targetedCherries;
    }

    public List<Collider> GetVisibleCherries()
    {
        return visibleCherries;
    }
}
