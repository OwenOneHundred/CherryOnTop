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

    float bigChance = 0.6f;

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
        int totalCherries = Mathf.RoundToInt(scaleFactor * defaultCherriesPerRound * (roundNumberIsOdd ? oddNumberCherryCountMultiplier : 1));
        RoundManager.roundManager.totalCherriesThisRound = totalCherries;
        float timeBetweenCherries = defaultTimeBetweenCherries / (scaleFactor * (roundNumberIsOdd ? 1 : evenNumberCherrySpacingMultiplier));

        bigChance = Mathf.Clamp(bigChance * 1.2f, 0, 100);

        for (int i = 0; i < totalCherries; i++)
        {
            GameObject newCherry = Instantiate(cherryPrefab, cherryStartPos, Quaternion.identity);
            cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());
            ApplyVariants(newCherry);

            yield return new WaitForSeconds(timeBetweenCherries);
        }
    }

    void ApplyVariants(GameObject cherry)
    {
        CherryTypes cherryTypes = cherry.GetComponent<CherryTypes>();

        float rng = Random.Range(0, bigChance);

        if (rng > 1)
        {
            cherryTypes.cherrySize = CherryTypes.CherrySize.Large;
        }
        else if (rng > 4)
        {
            cherryTypes.cherrySize = CherryTypes.CherrySize.SuperLarge;
        }
        else
        {
            cherryTypes.cherrySize = CherryTypes.CherrySize.Normal;
        }
        
        cherryTypes.SetCherryHealthAndSpeed();
    }
}
