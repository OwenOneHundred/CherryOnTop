using UnityEngine;

public class BoulderProjectile : Projectile
{
    LineRenderer line;
    int pointIndex = 0;
    int currentTarget = 0;
    Vector3[] linePositions;
    [SerializeField] float speed = 10;
    void Start()
    {
        GetClosestLineRendererPoint();
        currentTarget = pointIndex;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, linePositions[currentTarget], speed * Time.deltaTime);
        if (transform.position == linePositions[currentTarget])
        {
            currentTarget--;

            if (currentTarget < 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void GetClosestLineRendererPoint()
    {
        float closestDistance = 9999999;
        int closestIndex = 0;
        LineRenderer closestLineRenderer = null;
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
                }
                budgetEnum += 1;
            }
        }
        pointIndex = closestIndex;
        line = closestLineRenderer;
        linePositions = new Vector3[line.positionCount];
        line.GetPositions(linePositions);
    }

    public override Vector3 GetAttackDirection(GameObject attackedObject)
    {
        return (linePositions[currentTarget] - transform.position).normalized * speed;
    }
}
