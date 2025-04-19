using UnityEngine;

public class FloatUpAndDown : MonoBehaviour
{
    Vector3 startPos;
    [SerializeField] Vector3 moveDistance;
    Vector3 goalPos;
    bool movingUp = true;
    [SerializeField] float speed = 5;

    void Start()
    {
        startPos = transform.position;
        goalPos = startPos + moveDistance / 2;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime);
        if (transform.position == goalPos)
        {
            if (movingUp)
            {
                goalPos = startPos + moveDistance / 2;
            }
            else
            {
                goalPos = startPos - moveDistance / 2;
            }
            movingUp = !movingUp;
        }
    }
}
