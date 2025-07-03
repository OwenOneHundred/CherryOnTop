using UnityEngine;

public class SpinConstantly : MonoBehaviour
{
    [SerializeField] Vector3 spinSpeed;

    private void Update()
    {
        transform.Rotate(spinSpeed * Time.deltaTime);
    }
}
