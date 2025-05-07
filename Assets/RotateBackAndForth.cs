using UnityEngine;

public class RotateBackAndForth : MonoBehaviour
{
    float startRotation;
    [SerializeField] float rotateDistance = 90;
    float goalRotation;
    bool movingUp = true;
    [SerializeField] float speed = 5;

    void Start()
    {
        startRotation = transform.rotation.eulerAngles.z;
        goalRotation = startRotation + (rotateDistance / 2);
    }

    void Update()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(currentRotation, goalRotation, speed * Time.deltaTime));
        Debug.Log("current: " + currentRotation + " goal: " + goalRotation);
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
            movingUp = !movingUp;
        }
    }
}
