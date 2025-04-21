using UnityEngine;

public class ArrowMover : MonoBehaviour
{
    private GameObject track;
    [System.NonSerialized] public LineRenderer lineRenderer;
    [System.NonSerialized] public Vector3[] linePositions;
    private int currentTarget;
    [System.NonSerialized] public int currentPosition;
    [System.NonSerialized] public int currentTrack;
    [System.NonSerialized] public float baseSpeed = 1f;
    private int positionsAmount;
    [System.NonSerialized] public float distanceTraveled = 0f;
    private Vector3 previousCoords;

    bool movingToNewTrack = false;
    [SerializeField] float speed = 10;
    bool goBackToPosition0 = true;

    private void Start()
    {
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

        Vector3 newDirection = Vector3.RotateTowards(-transform.forward, linePositions[currentTarget] - transform.position, 100, 0.0f);
        transform.rotation = Quaternion.LookRotation(-newDirection);

        goBackToPosition0 = track.GetComponent<ArrowSpawner>().goBackToPosition0;
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
        Vector3 goalPos = linePositions[currentTarget];
        transform.position = Vector3.MoveTowards(transform.position, goalPos, speed * Time.deltaTime);
        Vector3 newDirection = Vector3.RotateTowards(-transform.forward, goalPos - transform.position, 5 * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(-newDirection);

        if (transform.position == goalPos) // if reached target position
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
        Destroy(gameObject);
    }

    private void SetNewTrack() {
        lineRenderer = track.transform.GetChild(currentTrack).GetComponent<LineRenderer>();
        positionsAmount = lineRenderer.positionCount;
        linePositions = new Vector3[positionsAmount];
        lineRenderer.GetPositions(linePositions);
    }
}
