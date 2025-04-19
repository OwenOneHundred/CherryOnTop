using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CherrySpawner : MonoBehaviour
{
    readonly float defaultTimeBetweenCherries = 1;
    readonly float defaultCherriesPerRound = 10;

    readonly float oddNumberCherryCountMultiplier = 1.5f;
    readonly float evenNumberCherrySpacingMultiplier = 1.5f;
    [SerializeField] public GameObject cherryPrefab;
    [SerializeField] Vector3 cherryStartPos;
    public float difficultyScalingAmount = 1.16f;
    [System.NonSerialized] public CherryManager cherryManager;

    float bigChance = 0.6f;
    public float initialBigChance = 0.6f;

    float specialtyCherryChance = 0.02f;
    float specialtyCherryChanceScaleMultiplier = 2f;
    readonly int specialtyCherryRoundCap = 8;
    public List<SpecialtyCherry> specialtyCherries = new List<SpecialtyCherry>();
    [SerializeField] bool doSpecialtyCherrySpawning = false;

    private void Start()
    {
        cherryManager = GetComponent<CherryManager>();
        if (!doSpecialtyCherrySpawning) { specialtyCherryChance = 0; }
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

        bigChance = Mathf.Clamp(initialBigChance * Mathf.Pow(difficultyScalingAmount, RoundManager.roundManager.roundNumber - 1), 0, 100);

        Debug.Log("Chance of specialty round: " + specialtyCherryChance);
        
        SpecialtyCherry specialtyCherryType = GetIsSpecialtyCherryRound();
        if (specialtyCherryType != null) { Debug.Log("Specialty cherry round!"); }

        for (int i = 0; i < totalCherries; i++)
        {
            GameObject prefab = cherryPrefab;
            if (specialtyCherryType != null && Random.value <= 0.5f)
            {
                prefab = specialtyCherryType.prefab;
            }
            if (doSpecialtyCherrySpawning && Random.value <= 0.01f)
            {
                prefab = GetRandomSpecialtyCherry().prefab;
            }
            GameObject newCherry = Instantiate(prefab, cherryStartPos, Quaternion.identity);
            cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());
            ApplyVariants(newCherry);

            yield return new WaitForSeconds(timeBetweenCherries);
        }
    }

    public SpecialtyCherry GetIsSpecialtyCherryRound()
    {
        if (RoundManager.roundManager.roundNumber < specialtyCherryRoundCap)
        {
            return null;
        }
        specialtyCherryChance *= specialtyCherryChanceScaleMultiplier;

        if (UnityEngine.Random.value <= specialtyCherryChance)
        {
            specialtyCherryChance = 0;
            return GetRandomSpecialtyCherry();
        }
        return null;
    }

    private SpecialtyCherry GetRandomSpecialtyCherry()
    {
        return specialtyCherries[GeneralUtil.RandomWeighted(specialtyCherries.Select(x => x.weight).ToList())];
    }

    void ApplyVariants(GameObject cherry)
    {
        CherryTypes cherryTypes = cherry.GetComponent<CherryTypes>();

        float rng = Random.Range(0, bigChance);

        if (rng > 4)
        {
            cherryTypes.cherrySize = CherryTypes.CherrySize.SuperLarge;
        }
        else if (rng > 1)
        {
            cherryTypes.cherrySize = CherryTypes.CherrySize.Large;
        }
        else
        {
            cherryTypes.cherrySize = CherryTypes.CherrySize.Normal;
        }
        
        cherryTypes.SetCherryHealthAndSpeed();
    }
}
