using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CherrySpawner : MonoBehaviour
{
    int roundNumber = 1;

    readonly float defaultTimeBetweenCherries = 1;
    readonly float defaultCherriesPerRound = 10;

    readonly float oddNumberCherryCountMultiplier = 1.5f;
    readonly float evenNumberCherrySpacingMultiplier = 1.5f;
    [SerializeField] GameObject cherryPrefab;
    [SerializeField] Vector3 cherryStartPos;

    void Start()
    {
        OnRoundStart(); // for testing
    }
    
    void OnRoundStart()
    {
        StartCoroutine(RoundCoroutine());
        roundNumber += 1;
    }

    IEnumerator RoundCoroutine()
    {
        bool roundNumberIsOdd = roundNumber % 2 == 1;
        float scaleFactor = (Mathf.Pow(roundNumber, 1.1f) / 2.5f) + 0.6f;
        float totalCherries = scaleFactor * defaultCherriesPerRound * (roundNumberIsOdd ? oddNumberCherryCountMultiplier : 1);
        float timeBetweenCherries = defaultTimeBetweenCherries / (scaleFactor * (roundNumberIsOdd ? 1 : evenNumberCherrySpacingMultiplier));

        for (int i = 0; i < totalCherries; i++)
        {
            Instantiate(cherryPrefab, cherryStartPos, Quaternion.identity);

            yield return new WaitForSeconds(timeBetweenCherries);
        }

        yield return new WaitForSeconds(3); // for testing

        OnRoundStart(); // this too
    }
}
