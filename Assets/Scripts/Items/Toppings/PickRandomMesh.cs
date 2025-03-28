using System.Collections.Generic;
using UnityEngine;

public class PickRandomMesh : MonoBehaviour
{
    [SerializeField] List<Mesh> meshes;

    void Start()
    {
        GetComponentInChildren<MeshFilter>().sharedMesh = meshes[Random.Range(0, meshes.Count)];
    }
}
