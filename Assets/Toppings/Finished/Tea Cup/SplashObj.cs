using UnityEngine;

public class SplashObj : MonoBehaviour
{
    public float lifetime = 2;
    public float speed = 1;
    public float maxSize = 1;
    [SerializeField] bool includeY = false;
    [SerializeField] LayerMask cake;

    void Start()
    {
        transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, cake))
        {   
            float distanceToGround = hit.distance;
            transform.position -= new Vector3(0, distanceToGround - 0.1f, 0);
        }
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (transform.localScale.x < maxSize)
        {
            transform.localScale = new Vector3(
                Mathf.Clamp(transform.localScale.x + speed * Time.deltaTime, 0, maxSize),
                includeY ? Mathf.Clamp(transform.localScale.y + speed * Time.deltaTime, 0, maxSize) : transform.localScale.y,
                Mathf.Clamp(transform.localScale.z + speed * Time.deltaTime, 0, maxSize));
        }
        
    }
}
