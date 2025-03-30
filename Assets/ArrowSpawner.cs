using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    float timer = 0;
    readonly float timeBetweenArrowSets = 8;
    readonly float timeBetweenEachArrowInSet = 0.35f;
    readonly int arrowsPerSet = 3;
    int arrowsSpawnedThisSet = 0;
    [SerializeField] GameObject arrowPrefab;

    Vector3 trackStartPos;

    void Start()
    {
        trackStartPos = GameObject.FindGameObjectWithTag("Track").transform.GetChild(0).GetComponent<LineRenderer>().GetPosition(0);
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
