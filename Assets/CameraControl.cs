using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float speed = 10;
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime * Input.GetAxisRaw("Horizontal"), 0);
    }
}
