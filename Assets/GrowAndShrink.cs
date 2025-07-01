using UnityEngine;

public class GrowAndShrink : MonoBehaviour
{
    float startSize;
    [SerializeField] float sizeChange = 0.2f;
    float goalSize;
    bool movingUp = true;
    [SerializeField] float speed = 0.2f;

    void Start()
    {
        startSize = transform.localScale.x;
        goalSize = startSize + (sizeChange / 2);
    }

    void Update()
    {
        float size = Mathf.MoveTowards(transform.localScale.x, goalSize, speed * Time.deltaTime);
        transform.localScale = new Vector3(size, size, 1);
        if (Mathf.Abs(size - goalSize) < 0.01f)
        {
            if (movingUp)
            {
                goalSize = startSize + (sizeChange / 2);
            }
            else
            {
                goalSize = startSize - (sizeChange / 2);
            }
            movingUp = !movingUp;
        }
    }
}
