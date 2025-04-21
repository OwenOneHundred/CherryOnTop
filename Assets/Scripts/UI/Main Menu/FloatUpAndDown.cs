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
        Debug.Log(moveDistance);
        startPos = transform.position;
        goalPos = startPos + (moveDistance / 2);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, goalPos) < 0.01f)
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
