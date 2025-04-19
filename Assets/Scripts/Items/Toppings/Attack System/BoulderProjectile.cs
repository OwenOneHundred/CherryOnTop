using System.Linq;
using UnityEngine;

public class BoulderProjectile : Projectile
{
    LineRenderer line;
    int currentTarget = 0;
    Vector3[] linePositions;
    [SerializeField] float speed = 10;
    int lineRendererNumber = 0;
    void Start()
    {
        GetClosestLineRendererPoint();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, linePositions[currentTarget], speed * Time.deltaTime);
        if (transform.position == linePositions[currentTarget])
        {
            currentTarget--;

            if (currentTarget < 0)
            {
                OnReachEndOfTrack();
            }
        }
    }

    private void OnReachEndOfTrack()
    {
        if (lineRendererNumber == 0)
        {
            // if there's a problem with the end of the track, it's probably this.
            // this line assumes that if cherries are supposed to go back to the start of each track before they jump,
            // The FIRST track is a circle. That might not necessarily be true.
            if (TrackFunctions.trackFunctions.gameObject.GetComponent<ArrowSpawner>().goBackToPosition0)
            {
                currentTarget = linePositions.Length - 1;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            lineRendererNumber -= 1;

            line = TrackFunctions.trackFunctions.tracks[lineRendererNumber];
            linePositions = new Vector3[line.positionCount];
            line.GetPositions(linePositions);

            currentTarget = linePositions.Length - 1;
        }
    }

    void GetClosestLineRendererPoint()
    {
        float closestDistance = 9999999;
        int closestIndex = 0;
        LineRenderer closestLineRenderer = null;
        int lineRendererNum = 0;
        int lineRendererEnum = 0;
        foreach (LineRenderer lineRenderer in GameObject.FindGameObjectWithTag("Track").GetComponentsInChildren<LineRenderer>())
        {
            int positionsAmount = lineRenderer.positionCount;
            var linePositions = new Vector3[positionsAmount];
            lineRenderer.GetPositions(linePositions);

            int budgetEnum = 0;
            foreach (Vector3 linePosition in linePositions)
            {
                float distance = Vector3.Distance(linePosition, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = budgetEnum;
                    closestLineRenderer = lineRenderer;
                    lineRendererNum = lineRendererEnum;
                }
                budgetEnum += 1;
            }
            lineRendererEnum += 1;
        }
        currentTarget = closestIndex;
        lineRendererNumber = lineRendererNum;
        line = closestLineRenderer;
        linePositions = new Vector3[line.positionCount];
        line.GetPositions(linePositions);
    }

    public override Vector3 GetAttackDirection(GameObject attackedObject)
    {
        return (linePositions[currentTarget] - transform.position).normalized * speed;
    }
}
