using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ArrowSpawner : MonoBehaviour
{
    float timer = 0;
    readonly float timeBetweenArrowSets = 8;
    readonly float timeBetweenEachArrowInSet = 0.35f;
    readonly int arrowsPerSet = 3;
    int arrowsSpawnedThisSet = 0;
    [SerializeField] GameObject arrowPrefab;
    
    public bool goBackToPosition0 = true;

    Vector3 trackStartPos;

    void Start()
    {
        trackStartPos = GameObject.FindGameObjectWithTag("Track").transform.GetChild(0).GetComponent<LineRenderer>().GetPosition(0);
        timer = timeBetweenArrowSets / 2;

        //ResizeLineRenderer(1.14285714286f, false);
    }

    // I have this here as a utility in case I need it lol
    private void ResizeLineRenderer(float newSize, bool includeY = false)
    {
        LineRenderer lineRenderer = GetComponentInChildren<LineRenderer>();
        int positionsAmount = lineRenderer.positionCount;
        var linePositions = new Vector3[positionsAmount];
        lineRenderer.GetPositions(linePositions);
        List<Vector3> posList = linePositions.ToList();
        for (int i = 0; i < positionsAmount; i++)
        {
            posList[i] = new Vector3(posList[i].x * newSize, (includeY ? posList[i].y * newSize : posList[i].y), posList[i].z * newSize);
        }
        lineRenderer.SetPositions(posList.ToArray());
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeBetweenArrowSets)
        {
            if (timer > timeBetweenArrowSets + (arrowsSpawnedThisSet * timeBetweenEachArrowInSet))
            {
                arrowsSpawnedThisSet += 1;
                SpawnArrow();
                if (arrowsSpawnedThisSet >= arrowsPerSet)
                {
                    timer = 0;
                    arrowsSpawnedThisSet = 0;
                }
            }
        }

        if (RoundManager.roundManager.roundNumber > 0)
        {
            this.enabled = false;
        }
    }   

    private void SpawnArrow()
    {
        Instantiate(arrowPrefab, trackStartPos, Quaternion.identity);
    }
}
