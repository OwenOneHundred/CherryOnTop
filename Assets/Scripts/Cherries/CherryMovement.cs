using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

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

    private GameObject track;
    public LineRenderer lineRenderer;
    public Vector3[] linePositions;
    private int currentTarget;
    public int currentPosition;
    public int currentTrack;
    public float baseSpeed = 1f;
    private int positionsAmount;
    public float distanceTraveled = 0f;
    private Vector3 previousCoords;

    DebuffManager debuffManager;

    private void Start()
    {
        debuffManager = GetComponent<DebuffManager>();

        track = GameObject.FindGameObjectWithTag("Track");
        lineRenderer = track.transform.GetChild(0).GetComponent<LineRenderer>();
        positionsAmount = lineRenderer.positionCount;
        linePositions = new Vector3[positionsAmount];
        lineRenderer.GetPositions(linePositions);
        currentPosition = 0;
        currentTarget = currentPosition + 1;
        currentTrack = 0;

        transform.position = linePositions[currentPosition];
        previousCoords = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, linePositions[currentTarget], GetSpeed() * Time.deltaTime);
        distanceTraveled += Vector3.Distance(previousCoords, transform.position);
        if (transform.position == linePositions[currentTarget])
        {
            //progress = 0;
            currentPosition++;
            currentTarget++;
            if (currentTarget == positionsAmount) 
            {
                currentTarget = 0;
            }
            if (currentPosition == positionsAmount) 
            {
                currentTrack++;
                if (track.transform.childCount <= currentTrack) { OnReachEnd(); return; }
                lineRenderer = track.transform.GetChild(currentTrack).GetComponent<LineRenderer>();
                //If current position is -1, it is in the process of moving between lines.
                setNewTrack();
                currentPosition = -1;
                currentTarget = 0;
            }
        }
        previousCoords = transform.position;
    }

    private void OnReachEnd()
    {
        GameOverControl.gameOverControl.OnGameOver();
        Destroy(gameObject);
    }

    private void setNewTrack() {
        lineRenderer = track.transform.GetChild(currentTrack).GetComponent<LineRenderer>();
        positionsAmount = lineRenderer.positionCount;
        linePositions = new Vector3[positionsAmount];
        lineRenderer.GetPositions(linePositions);
    }

    private float GetSpeed()
    {
        Debug.Log("Movement speed: " + baseSpeed * debuffManager.GetMovementSpeedMultiplier());
        return baseSpeed * debuffManager.GetMovementSpeedMultiplier();
    }
}
