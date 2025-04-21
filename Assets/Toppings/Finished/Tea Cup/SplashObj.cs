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
        if (Physics.BoxCast(transform.position + new Vector3(0, 2, 0), new Vector3(0.2f, 0.2f, 0.2f), Vector3.down, out hit, Quaternion.identity, 10, cake))
        {   
            transform.position = hit.point + new Vector3(0, 0.1f, 0);
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
