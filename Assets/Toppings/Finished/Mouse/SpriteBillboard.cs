using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 direction = cam.transform.position - transform.position;
        direction.y = 0;
        transform.forward = direction;
    }
}
