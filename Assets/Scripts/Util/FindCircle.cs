using System.Collections.Generic;
using UnityEngine;

public class FindCircle : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float betweenPoints = 0.05f;

    void Start()
    {
        Vector3[] points = GetPointsOnUnitCircle(betweenPoints).ToArray();
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    List<Vector3> GetPointsOnUnitCircle(float betweenPoints)
    {
        List<Vector3> points = new List<Vector3>();
        float circumference = 2 * Mathf.PI;
        int numPoints = (int)(circumference / betweenPoints);
        float angleStep = 2 * Mathf.PI / numPoints;
        
        for (int i = 0; i < numPoints; i++)
        {
            float angle = i * angleStep;
            points.Add(new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle)));
        }
        points.Add(points[0]);
        
        return points;
    }
}
