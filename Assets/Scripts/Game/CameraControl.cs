using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float keyMovementSpeed = 10;
    [SerializeField] float mouseScrollMovementSpeed = 10;
    float shakeTimer = 0;
    float shakeTime = 0;
    float shakeViolence = 0.2f;
    bool shaking = false;
    void Update()
    {
        DoRotation();

        if (shaking)
        {
            transform.position = Random.insideUnitCircle * shakeViolence;
            shakeTimer += Time.timeScale == 0 ? Time.unscaledDeltaTime : Time.deltaTime;
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

    private void DoRotation()
    {
        if (ToppingPlacer.toppingPlacer.PlacingTopping && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            return;
        }

        Vector3 eulerAngles = transform.rotation.eulerAngles;
        float intendedHorizontalRotation = eulerAngles.y + keyMovementSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
        float intendedVerticalRotation = eulerAngles.x + keyMovementSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");
        if (intendedVerticalRotation > 45 && intendedVerticalRotation < 120) { intendedVerticalRotation = 45; }
        if (intendedVerticalRotation < 315 && intendedVerticalRotation > 200) { intendedVerticalRotation = 315; }
        transform.rotation = Quaternion.Euler(intendedVerticalRotation, intendedHorizontalRotation, 0);

        transform.Rotate(0, mouseScrollMovementSpeed * Time.deltaTime * Input.mouseScrollDelta.y, 0);
    }

    public void ApplyCameraShake(float length, float violence = 1)
    {
        if (length == 0 || violence == 0) { return; }
        if (length < shakeTime - shakeTimer) { return; }

        shakeTime = length;
        shakeViolence = violence;
        shaking = true;
    }
}

