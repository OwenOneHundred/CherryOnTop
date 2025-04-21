using System.Linq;
using Unity.VisualScripting;
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
    public int currentTarget;
    public int currentPosition;
    public int currentTrack;
    public float baseSpeed = 1f;
    private int positionsAmount;
    public float distanceTraveled = 0f;
    private Vector3 previousCoords;
    bool goBackToPosition0;

    DebuffManager debuffManager;

    bool movingToNewTrack = false;

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
        goBackToPosition0 = track.GetComponent<ArrowSpawner>().goBackToPosition0;

        transform.position = linePositions[currentPosition];
        previousCoords = transform.position;
    }

    private void Update()
    {
        if (movingToNewTrack)
        {
            WhileJumping();
        }
        else
        {
            MoveAlongTrack();
        }

        RecordChangeInDistance();
    }

    void MoveAlongTrack()
    {
        transform.position = Vector3.MoveTowards(transform.position, linePositions[currentTarget], GetSpeed() * Time.deltaTime);

        if (transform.position == linePositions[currentTarget]) // if reached target position
        {
            currentPosition++;
            currentTarget++;
            if (currentTarget == positionsAmount) // at last position, targeting non-existent position
            {
                currentTarget = 0;
                if (!goBackToPosition0)
                {
                    OnReachEndOfTrack();
                }
            }
            if (currentPosition == positionsAmount) // returned to position 0
            {
                OnReachEndOfTrack();
            }
        }
    }

    Vector3 goalJumpPosition;
    Vector3 jumpStartPosition;
    float goalJumpY;
    float goalJumpTime;
    readonly float verticalJumpClearance = 1.5f;
    float velocityY;
    float jumpHorizontalSpeed;
    float gravity = 9.8f;
    float timer = 0;
    void StartJump() // set goal height, decceleration, calculate time based on distance, calculate initial velocity based on others
    {
        timer = 0; 

        goalJumpPosition = linePositions[currentTarget];
        jumpStartPosition = transform.position;

        float heighestYBetweenGoalAndStart = (jumpStartPosition.y > goalJumpPosition.y) ? jumpStartPosition.y : goalJumpPosition.y;
        goalJumpY = heighestYBetweenGoalAndStart + verticalJumpClearance;

        goalJumpTime = Mathf.Clamp(Vector3.Distance(goalJumpPosition, jumpStartPosition), 1.5f, 8) / 3;
        
        velocityY = CalculateInitialVelocityKinFormula(Mathf.Abs(goalJumpY - jumpStartPosition.y), -gravity, goalJumpTime);

        jumpHorizontalSpeed = Vector2.Distance(new Vector2(goalJumpPosition.x, goalJumpPosition.z), new Vector2(jumpStartPosition.x, jumpStartPosition.z)) / goalJumpTime;

        static float CalculateInitialVelocityKinFormula(float distance, float acceleration, float time)
        {
            return (distance - (0.5f * acceleration * (time * time))) / time;
        }
    }
    void WhileJumping()
    {
        timer += Time.deltaTime;
        if (timer >= goalJumpTime * 0.9f && transform.position.y < goalJumpPosition.y)
        {
            transform.position = goalJumpPosition;
            movingToNewTrack = false;
        }

        Vector2 xzMovement = (new Vector2(goalJumpPosition.x, goalJumpPosition.z) - new Vector2(transform.position.x, transform.position.z)).normalized * jumpHorizontalSpeed * Time.deltaTime;
        transform.position += new Vector3(xzMovement.x, velocityY * Time.deltaTime, xzMovement.y);
        velocityY -= gravity * Time.deltaTime;
    }

    void RecordChangeInDistance()
    {
        distanceTraveled += Vector3.Distance(previousCoords, transform.position);
        previousCoords = transform.position;
    }

    void OnReachEndOfTrack()
    {
        currentTrack++;
        if (track.transform.childCount <= currentTrack)
        {
            OnReachEndOfMap();
            return;
        }

        lineRenderer = track.transform.GetChild(currentTrack).GetComponent<LineRenderer>();

        SetNewTrack();

        movingToNewTrack = true;
        currentPosition = -1;
        currentTarget = 0;

        StartJump();
    }

    private void OnReachEndOfMap()
    {
        if (GameOverControl.gameOverControl.isGameOver)
        {
            Destroy(gameObject);
        }
        else
        {
            GameOverControl.gameOverControl.OnGameOver(gameObject);
            this.enabled = false;
        }
    }

    private void SetNewTrack() {
        lineRenderer = track.transform.GetChild(currentTrack).GetComponent<LineRenderer>();
        positionsAmount = lineRenderer.positionCount;
        linePositions = new Vector3[positionsAmount];
        lineRenderer.GetPositions(linePositions);
    }

    private float GetSpeed()
    {
        return baseSpeed * debuffManager.GetMovementSpeedMultiplier();
    }
}
