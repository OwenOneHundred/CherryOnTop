using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public float speed = 10;
    bool open;
    bool moving;
    readonly float closedPos = -1690;
    readonly float openPos = -230;
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void ToggleOpen()
    {
        Open = !Open;
    }

    public bool Open
    {
        get { return open; }
        private set
        {
            if (open == value) { return; }
            if (moving) { return; }

            open = value;
            moving = true;

            StartCoroutine(Mover());
        }
    }

    public IEnumerator Mover()
    {
        float goal = open ? openPos : closedPos;

        while (Mathf.Abs(rect.anchoredPosition.x - goal) >= 0.01f)
        {
            rect.anchoredPosition =
                Vector2.MoveTowards(rect.anchoredPosition, new Vector2(goal, rect.anchoredPosition.y), speed * Time.deltaTime);
            yield return null;
        }
        rect.anchoredPosition = new Vector2(goal, rect.anchoredPosition.y);

        moving = false;
    }
}
