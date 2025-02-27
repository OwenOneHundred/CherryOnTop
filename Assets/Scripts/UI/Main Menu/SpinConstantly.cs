using UnityEngine;

public class SpinConstantly : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void FixedUpdate()
    {
        // Rotate the object around the Y axis
        transform.Rotate(Vector3.up, 1f);
    }
}
