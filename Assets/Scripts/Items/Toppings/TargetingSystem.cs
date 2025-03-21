using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    private LayerMask cherryLayer, cakeLayer;
    [SerializeField] AttackManager attackManager;
 

    void Start()
    {
        cherryLayer = LayerMask.GetMask("Cherry"); // Only detect enemies
        cakeLayer = LayerMask.GetMask("Cake"); // Walls
    }

    void Update()
    {
        GameObject found = Search();
        attackManager.UpdateTargetedCherry(found);
    }

   GameObject Search()
    {
        GameObject bestCherry = null;
        float highestDistance = -1f;

        Collider[] cherries = Physics.OverlapSphere(transform.position, range, cherryLayer);

        foreach (Collider cherry in cherries)
        {
            CherryMovement cherryMovement = cherry.GetComponent<CherryMovement>();
            if (cherryMovement != null && HasClearLineOfSight(cherry.transform))
            {
                if (cherryMovement.distanceTraveled > highestDistance)
                {
                    highestDistance = cherryMovement.distanceTraveled;
                    bestCherry = cherry.gameObject;
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

}
