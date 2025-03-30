using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float speed = 10;
    float shakeTimer = 0;
    float shakeTime = 0;
    float shakeViolence = 0.2f;
    bool shaking = false;
    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime * Input.GetAxisRaw("Horizontal"), 0);

        if (shaking)
        {
            transform.position = Random.insideUnitCircle * shakeViolence;
            shakeTimer += Time.deltaTime;
            if (shakeTimer >= shakeTime)
            {
                shakeTimer = 0;
                shakeTime = 0;
                shaking = false;
            }
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

    public void ApplyCameraShake(float length, float violence = 1)
    {
        if (length < shakeTime - shakeTimer) { return; }

        shakeTime = length;
        shakeViolence = violence;
        shaking = true;
    }
}

