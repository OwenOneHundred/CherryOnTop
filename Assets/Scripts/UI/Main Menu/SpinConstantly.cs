using UnityEngine;

public class SpinConstantly : MonoBehaviour
{
    [SerializeField] float spinSpeed = 1;

    private void FixedUpdate()
    {
        // Rotate the object around the Y axis
        transform.Rotate(Vector3.up, spinSpeed);
    }
}
