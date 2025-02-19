using UnityEngine;

/// <summary>
/// Attached to Cherries. Moves them along the track. Should only interact with the debuff manager on this Cherry,
/// and only to access a function to get a multiplier for movement speed, to allow for slow effects.
/// </summary>
public class CherryMovement : MonoBehaviour
{
    /// The track should be a (maybe disabled) linerenderer.
    /// The cherries should move from one point on the linerenderer to the next, which allows for easy
    /// track path adjustments. However, there should be a way to freeze cherries normal movement and have them
    /// "jump" to a new location. That's how they will move to a new layer, and it is important for level design.


    public LineRenderer lineRenderer;
    public Vector3[] linePositions;
    private int currentTarget;
    public int currentPosition;
    public float progress = 0f;
    public float speed = 1f;
    private int positionsAmount;
    //private Vector3 actualPosition;

    [System.Obsolete]
    private void Start()
    {
        lineRenderer = Object.FindAnyObjectByType<LineRenderer>();
        positionsAmount = lineRenderer.positionCount;
        linePositions = new Vector3[positionsAmount];
        lineRenderer.GetPositions(linePositions);
        currentPosition = 0;
        currentTarget = currentPosition + 1;
        print("positionsAmount: " + positionsAmount);
        //actualPosition = linePositions[currentPosition];
    }

    private void Update()
    {
        //Vector3 actualTarget = linePositions[currentTarget];
        //actualTarget.y = Mathf.Cos(Time.deltaTime * 5) * 10;
        transform.position = Vector3.Lerp(linePositions[currentPosition], linePositions[currentTarget], progress);
        progress += Time.deltaTime * 1f;
        if (progress >= 1f)
        {
            progress = 0;
            currentPosition++;
            currentTarget++;
            if (currentTarget == positionsAmount) 
            {
                currentTarget = 0;
            }
            if (currentPosition == positionsAmount) 
            {
                currentPosition = 0;
                currentTarget = 1;
            }
        }

    }

}
