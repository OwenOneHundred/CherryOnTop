using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrackFunctions : MonoBehaviour
{
    List<Vector3[]> trackPositions = new();
    public int PositionsAmount { get; private set; }
    List<LineRenderer> tracks = new();
    public static TrackFunctions trackFunctions;

    // Represents the maximum number of units up or down the track can be from the topping's location.
    //private const float VERT_DISTANCE_THRESHOLD = 10;
    
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


    public List<LineSegment3D> GetAllLineSegmentsThatIntersectSphere(Vector3 center, float radius)
    {
        List<LineSegment3D> lineSegments = new();
        foreach (Vector3[] trackPoints in trackPositions)
        {
            for (int i = 1; i < trackPoints.Length; i++)
            {
                Vector3 previous = trackPoints[i - 1];
                Vector3 current = trackPoints[i];
                float distance = GetDistanceToLineSegment3D(center, new LineSegment3D(previous, current));
                if (distance < radius) { lineSegments.Add(new LineSegment3D(previous, current)); }
            }
        }
        return lineSegments;
    }

    public static float GetDistanceToLineSegment3D(Vector3 origin, LineSegment3D ls)
    {
        return GetSimplifiedLineSegment3D(origin, ls).pointA.y;
    }

    public static LineSegment3D GetSimplifiedLineSegment3D(Vector3 origin, LineSegment3D ls)
    {
        Vector3 v = (ls.pointB - ls.pointA).normalized;
        LineSegment3D transls = new LineSegment3D(ls.pointA - origin, ls.pointB - origin);

        Vector3 normal = Vector3.Cross(transls.pointA.normalized, v);
        Vector3[] basisVectors = {v, Vector3.Cross(v, normal), normal};
        Matrix3D basis = new Matrix3D(basisVectors);

        return new LineSegment3D(GetBasisCoordinates(basis, transls.pointA), GetBasisCoordinates(basis, transls.pointB));

    }

    public static Vector3 GetBasisCoordinates(Matrix3D basis, Vector3 targetVector)
    {
        return MatrixMultiply(MatrixAdjugate(basis), targetVector);
    }

    public static Vector3 MatrixMultiply(Matrix3D matrix3, Vector3 vector) 
    {
        float x = matrix3.array[0,0] * vector.x + matrix3.array[0,1] * vector.y + matrix3.array[0,2] * vector.z;
        float y = matrix3.array[1,0] * vector.x + matrix3.array[1,1] * vector.y + matrix3.array[1,2] * vector.z;
        float z = matrix3.array[2,0] * vector.x + matrix3.array[2,1] * vector.y + matrix3.array[2,2] * vector.z;
        return new Vector3(x, y, z);
    }

    public static Matrix3D MatrixAdjugate(Matrix3D matrix3)
    {
        return MatrixTranspose(MatrixCofactor(matrix3));
    }

    public static Matrix3D MatrixTranspose(Matrix3D matrix3)
    {
        Matrix3D newMatrix = new Matrix3D();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                newMatrix.array[i,j] = matrix3.array[j,i];
            }
        }
        return newMatrix;
    }

    public static Matrix3D MatrixCofactor(Matrix3D matrix3)
    {
        Matrix3D cofactorMatrix = new Matrix3D(null);
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float[,] matrix2 = {
                { matrix3.array[(i + 1) % 3,(j + 1) % 3]  ,
                  matrix3.array[(i + 1) % 3,(j + 2) % 3] },
                { matrix3.array[(i + 2) % 3,(j + 1) % 3]  ,
                  matrix3.array[(i + 2) % 3,(j + 2) % 3] }};
                cofactorMatrix.array[i,j] = MatrixDeterminant(matrix2);
            }
        }
        return cofactorMatrix;
    }

    public static float MatrixDeterminant(float[,] matrix2)
    {
        return matrix2[0,0] * matrix2[1,1] - matrix2[1,0] * matrix2[0,1];
    }

    /*
    public List<LineSegment2D> GetAllLineSegmentsThatIntersectCircle(Vector3 center, float radius)
    {
        List<LineSegment2D> lineSegments = new();
        foreach (Vector3[] trackPoints in trackPositions)
        {
            for (int i = 1; i < trackPoints.Length; i++) {
                Vector3 previous = trackPoints[i - 1];
                Vector3 current = trackPoints[i];
                float distance = GetDistanceToLineSegment(ToVector2D(center), new LineSegment2D(ToVector2D(previous), ToVector2D(current)));
                if (distance < radius && (Mathf.Abs(previous.y - center.y) < VERT_DISTANCE_THRESHOLD) || Mathf.Abs(current.y - center.y) < VERT_DISTANCE_THRESHOLD) {
                    lineSegments.Add(new LineSegment2D(ToVector2D(previous), ToVector2D(current)));
                }
            }
        }
        return lineSegments;
    }
    */

    /*
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
    */

    /*
    public static float GetDistanceToLineSegment2D(float ax, float ay, float bx, float by, float x, float y)
    {
        if ((ax-bx)*(x-bx)+(ay-by)*(y-by) <= 0)
            return Mathf.Sqrt((x - bx) * (x - bx) + (y - by) * (y - by));
        
        if ((bx-ax)*(x-ax)+(by-ay)*(y-ay) <= 0)
            return Mathf.Sqrt((x - ax) * (x - ax) + (y - ay) * (y - ay));
        
        return Mathf.Abs((by - ay)*x - (bx - ax)*y + bx*ay - by*ax) /
            Mathf.Sqrt((ay - by) * (ay - by) + (ax - bx) * (ax - bx));
    }
    */

    /*
    public static Vector2 ToVector2D(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    public static Vector3 ToVector3D(Vector2 v, float y)
    {
        return new Vector3(v.x, y, v.y);
    }
    */

    /*
    public static LineSegment2D GetSimplifiedLineSegment(Vector2 origin, LineSegment2D ls)
    {
        Vector2[] ROTATION_MATRIX = {new Vector2(0, 1), new Vector2(-1, 0)};
        Vector2 lsDirection = (ls.pointB - ls.pointA).normalized;

        LineSegment2D transls = new LineSegment2D(ToVector2D(ls.pointA - origin), ToVector2D(ls.pointB - origin)); // translated line segment
        Vector2[] basis = {lsDirection, MatrixMultiply(ROTATION_MATRIX, lsDirection)};

        return new LineSegment2D(GetBasisCoordinates(basis, transls.pointA), GetBasisCoordinates(basis, transls.pointB));
    }
    */

    /*
    public static float GetDistanceToLineSegment(Vector2 origin, LineSegment2D ls)
    {
        return GetSimplifiedLineSegment(origin, ls).pointA.y;
    }

    public static Vector2[] MatrixInverse(Vector2[] matrix)
    {
        float det = matrix[0].x * matrix[1].y - matrix[1].x * matrix[0].y;
        Vector2[] inverse = {(new Vector2(matrix[1].y, -1 * matrix[0].y)) / det, (new Vector2(-1 * matrix[1].x, matrix[0].x)) / det};
        return inverse;
    }
    */

    /*
    public Vector3 GetPositionByIndex(int track, int index)
    {
        return trackPositions[track][index];
    }
    */

    /*
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
    */

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
    
    /*
    public struct LineSegment2D
    {
        public Vector2 pointA;
        public Vector2 pointB;
        public float length;

        public LineSegment2D(Vector2 pointA, Vector2 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.length = (pointB - pointA).magnitude;
        }
    }
    */

    public struct LineSegment3D
    {
        public Vector3 pointA;
        public Vector3 pointB;
        public float length;

        public LineSegment3D(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.length = (pointB - pointA).magnitude;
        }
    }

    public struct Matrix3D
    {
        public float[,] array;
            
        public Matrix3D(Vector3[] vectors)
        {
            this.array = new float[3,3];
            if (vectors != null) {
                for (int i = 0; i < 3; i++)
                {
                    array[0,i] = vectors[i].x;
                    array[1,i] = vectors[i].y;
                    array[2,i] = vectors[i].z;
                }
            }
        }

        /*
        public Vector3[] getVectors()
        {
            Vector3[] vectors = new Vector3[3];
            for (int i = 0; i < 3; i++)
            {
                vectors[i].x = array[0,i];
                vectors[i].y = array[1,i];
                vectors[i].z = array[2,i];
            }
            return vectors;
        }
        */
    }

    /*
    public struct LineSegment
    {
        public Vector3 pointA;
        public Vector3 pointB;
        public float length;

        public LineSegment(Vector3 pointA, Vector3 pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.length = (pointB.position - pointA.position).magnitude;
        }
    }
    */
}
