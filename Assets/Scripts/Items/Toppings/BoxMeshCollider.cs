using UnityEngine;

/// <summary>
/// Attaches a BoxCollider and assigns it the size of the mesh being rendered on this object.
/// </summary>
public class BoxMeshCollider : MonoBehaviour
{
    BoxCollider boxCollider;
    void Start()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
        Bounds bounds = GetComponent<MeshFilter>().sharedMesh.bounds;
        boxCollider.size = bounds.size;
    }
}
