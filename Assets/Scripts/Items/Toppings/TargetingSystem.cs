using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    private LayerMask cherryLayer, cakeLayer;

    void Start()
    {
        cherryLayer = LayerMask.GetMask("Cherry"); // Only detect enemies
        cakeLayer = LayerMask.GetMask("Cake"); // Walls
    }

    void Update()
    {
        Search();
    }

    void Search()
    {
        Collider[] cherries = Physics.OverlapSphere(transform.position, range, cherryLayer);

        foreach (Collider cherry in cherries)
        {
            if (HasClearLineOfSight(cherry.transform))
            {
                Debug.Log("Target acquired: " + cherry.name);
                // Here you can call Shoot() or another attack method
            }
            else
            {
                Debug.Log("Target blocked: " + cherry.name);
            }
        }
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
        }

        Debug.DrawRay(transform.position, direction * distance, Color.yellow);
        return true; // Clear sight to Cherry
    }
}
