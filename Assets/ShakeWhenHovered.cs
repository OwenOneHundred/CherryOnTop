using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class ShakeWhenHovered : MonoBehaviour, IPointerEnterHandler
{
    float startRotation;
    [SerializeField] float rotateDistance = 90;
    float goalRotation;
    bool movingUp = true;
    [SerializeField] float speed = 5;
    public int rotationsPerHover = 6;
    int rotationsLeft = -1; // -1 means back at original position, 0 means done but needs to reset, > 0 means rotating
    [SerializeField] AudioFile hoverSound;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
        goalRotation = startRotation + (rotateDistance / 2);
    }

    void Update()
    {
        if (rotationsLeft > -1)
        {
            float currentRotation = transform.rotation.eulerAngles.z;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(currentRotation, goalRotation, speed * Time.deltaTime));
            float standardizedGoalRotation = goalRotation < 0 ? goalRotation + 360 : goalRotation;
            if (Mathf.Abs(currentRotation - standardizedGoalRotation) < 0.02f)
            {
                if (movingUp)
                {
                    goalRotation = startRotation + (rotateDistance / 2);
                }
                else
                {
                    goalRotation = startRotation - (rotateDistance / 2); 
                }

                rotationsLeft -= 1;
                if (rotationsLeft > 0)
                {
                    movingUp = !movingUp;
                }
                else if (rotationsLeft == 0)
                {
                    movingUp = !movingUp;
                    goalRotation = startRotation;
                }
                else
                {
                    movingUp = true;
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rotationsLeft != -1) { return; }
        rotationsLeft = rotationsPerHover;
        if (hoverSound.clip != null)
        {
            SoundEffectManager.sfxmanager.PlayOneShot(hoverSound);
        }
    }
}
