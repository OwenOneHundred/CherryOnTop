using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CherrySpawner : MonoBehaviour
{
    readonly float defaultTimeBetweenCherries = 1;
    readonly float defaultCherriesPerRound = 10;

    readonly float oddNumberCherryCountMultiplier = 1.5f;
    readonly float evenNumberCherrySpacingMultiplier = 1.5f;
    [SerializeField] GameObject cherryPrefab;
    [SerializeField] Vector3 cherryStartPos;
    CherryManager cherryManager;

    private void Start()
    {
        cherryManager = GetComponent<CherryManager>();
    }
    
    public void OnRoundStart()
    {
        StartCoroutine(RoundCoroutine());
    }

    IEnumerator RoundCoroutine()
    {
        bool roundNumberIsOdd = RoundManager.roundManager.roundNumber % 2 == 1;
        float scaleFactor = (Mathf.Pow(RoundManager.roundManager.roundNumber, 1.1f) / 2.5f) + 0.6f;
        float totalCherries = scaleFactor * defaultCherriesPerRound * (roundNumberIsOdd ? oddNumberCherryCountMultiplier : 1);
        RoundManager.roundManager.totalCherriesThisRound = Mathf.RoundToInt(totalCherries);
        float timeBetweenCherries = defaultTimeBetweenCherries / (scaleFactor * (roundNumberIsOdd ? 1 : evenNumberCherrySpacingMultiplier));

        for (int i = 0; i < totalCherries; i++)
        {
            GameObject newCherry = Instantiate(cherryPrefab, cherryStartPos, Quaternion.identity);
            cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());

            yield return new WaitForSeconds(timeBetweenCherries);
        }
    }
}
