using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackFunctions : MonoBehaviour
{
    List<Vector3[]> trackPositions = new();
    public int PositionsAmount { get; private set; }
    List<LineRenderer> tracks = new();
    public static TrackFunctions trackFunctions;
    
    void Awake()
    {
        if (trackFunctions == null || trackFunctions == this) { trackFunctions = this; }
        else { return; }

        GetAllTracks();

        SetTrackList();
    }

    public PointID GetClosestPointOnTrack(Vector3 pos)
    {
        float closestDistance = 100000;
        Vector3 closestPoint = Vector3.zero;
        int trackNumber = 0;
        int index = 0;
        int trackEnum;
        int indexEnum;

        trackEnum = 0;
        foreach (Vector3[] track in trackPositions)
        {
            indexEnum = 0;
            foreach (Vector3 position in track)
            {
                float distance = Vector3.Distance(pos, position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPoint = position;
                    index = indexEnum;
                    trackNumber = trackEnum;
                }
                indexEnum += 1;
            }
            trackEnum += 1;
        }
        return new PointID(closestPoint, trackNumber, index, closestDistance);
    }

    public List<PointID> GetClosestPointsOnTrack(Vector3 pos, int count)
    {
        List<PointID> closestPoints = new();

        int trackEnum = 0;
        foreach (Vector3[] track in trackPositions)
        {
            int indexEnum = 0;
            foreach (Vector3 position in track)
            {
                float distance = Vector3.Distance(pos, position);
                PointID newPoint = new PointID(position, trackEnum, indexEnum, distance);

                if (closestPoints.Count < count) // don't have enough points yet
                {
                    closestPoints.Add(newPoint);
                    closestPoints.Sort((a, b) => a.distanceAway.CompareTo(b.distanceAway)); // sort so last is worst
                }
                else if (distance < closestPoints[^1].distanceAway) // worse than worst point
                {
                    closestPoints[^1] = newPoint;
                    closestPoints.Sort((a, b) => a.distanceAway.CompareTo(b.distanceAway)); // sort so last is worst
                }

                indexEnum += 1;
            }
            trackEnum += 1;
        }

        return closestPoints;
    }

    public List<LineSegment> GetAllLineSegmentsThatIntersectCircle(Vector3 center, float radius)
    {
        List<LineSegment> lineSegments = new();
        int trackEnum = 0;
        foreach (Vector3[] trackPoints in trackPositions)
        {
            for (int posNum = 1; posNum < trackPoints.Length; posNum++)
            {
                PointID previous = new PointID(trackPoints[posNum - 1], trackEnum, posNum - 1, 0);
                PointID current = new PointID(trackPoints[posNum], trackEnum, posNum, 0);
                float distance = GetDistanceToLineSegment2D(previous.position.x, previous.position.z, current.position.x, current.position.z, center.x, center.z);
                if (distance < radius) { lineSegments.Add( new LineSegment(previous, current, distance)); }
            }
            trackEnum += 1;
        }
        return lineSegments.OrderBy(x => x.distanceAway).ToList();
    }   

    public static float GetDistanceToLineSegment2D(float ax, float ay, float bx, float by, float x, float y)
    {
        if ((ax-bx)*(x-bx)+(ay-by)*(y-by) <= 0)
            return Mathf.Sqrt((x - bx) * (x - bx) + (y - by) * (y - by));
        
        if ((bx-ax)*(x-ax)+(by-ay)*(y-ay) <= 0)
            return Mathf.Sqrt((x - ax) * (x - ax) + (y - ay) * (y - ay));
        
        return Mathf.Abs((by - ay)*x - (bx - ax)*y + bx*ay - by*ax) /
            Mathf.Sqrt((ay - by) * (ay - by) + (ax - bx) * (ax - bx));
    }

    public Vector3 GetPositionByIndex(int track, int index)
    {
        return trackPositions[track][index];
    }

    public Vector3 GetPreviousPosition(int track, int index)
    {
        index -= 1;
        if (index < 0)
        {
            track -= 1;
            if (track < 0)
            {
                return trackPositions[0][0];
            }
            index = tracks[track].positionCount - 1;
        }
        return trackPositions[track][index];
    }

    private void GetAllTracks()
    {
        foreach (Transform trans in transform)
        {
            if (trans.TryGetComponent<LineRenderer>(out LineRenderer lr))
            {
                tracks.Add(lr);
            }
        }
    }

    private void SetTrackList()
    {
        foreach (LineRenderer lineRenderer in tracks)
        {
            PositionsAmount += lineRenderer.positionCount;
            Vector3[] positions = new Vector3[lineRenderer.positionCount];
            lineRenderer.GetPositions(positions);
            trackPositions.Add(positions);
        }
    }

    public struct PointID
    {
        public Vector3 position;
        public int trackNumber;
        public int index;
        public bool initialized;
        public float distanceAway;

        public PointID(Vector3 position, int trackNumber, int index, float distanceAway)
        {
            this.position = position;
            this.trackNumber = trackNumber;
            this.index = index;
            initialized = true;
            this.distanceAway = distanceAway;
        }
    }

    public struct LineSegment
    {
        public PointID pointA;
        public PointID pointB;
        public float distanceAway;

        public LineSegment(PointID pointA, PointID pointB, float distanceAway)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.distanceAway = distanceAway;
        }
    }
}
