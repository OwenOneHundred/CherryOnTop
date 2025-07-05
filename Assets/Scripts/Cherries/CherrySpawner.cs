using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class CherrySpawner : MonoBehaviour
{
    readonly float defaultTimeBetweenCherries = 1;
    public float defaultCherriesPerRound = 8;

    [SerializeField] public GameObject cherryPrefab;
    [SerializeField] Vector3 cherryStartPos;
    [System.NonSerialized] public CherryManager cherryManager;

    float bigChance = 0.6f;
    public float initialBigChance = 0.6f;

    public List<SpecialtyCherry> specialtyCherries = new List<SpecialtyCherry>();

    public Difficulty difficulty;

    int metalCherryThreshold = 250;
    int mediumMetalCherryValue = 200;
    int largeMetalCherryValue = 600;
    int superLargeMetalCherryValue = 1000;

    private void Start()
    {
        cherryManager = GetComponent<CherryManager>();
        difficulty = DifficultyInfo.difficultyInfo.gameDifficultyParams.Difficulty;
    }

    public void OnRoundStart()
    {
        if (difficulty.TryGetHardCodedRound((int)RoundManager.roundManager.roundNumber, out HardCodedRound hardCodedRound))
        {
            StartCoroutine(HardCodedRoundCoroutine(hardCodedRound));
        }
        else
        {
            StartCoroutine(ProceduralRoundCoroutine());
        }
        
        
    }

    IEnumerator ProceduralRoundCoroutine()
    {
        float amountMultiplier = CalculateAmountScaleFactor(difficulty.cherryAmountScaleValue);
        int totalCherries = Mathf.RoundToInt(amountMultiplier * defaultCherriesPerRound);

        float timeBetweenCherries = defaultTimeBetweenCherries / amountMultiplier;

        bigChance = CalculateCherrySize(difficulty.cherrySizeScaleValue);

        RoundManager.roundManager.totalCherriesThisRound = totalCherries;

        int[] metalCherriesCountBySize = new int[4] { 0, 0, 0, 0 }; // small, medium, large, superLarge. Small is unused.
        if (totalCherries > metalCherryThreshold)
        {
            metalCherriesCountBySize = GetHowManyMetalCherries();
        }

        for (int i = 0; i < totalCherries; i++) 
        {
            SpawnCherry();
        
            yield return new WaitForSeconds(timeBetweenCherries);
        }

        if (metalCherriesCountBySize.Where(x => x != 0).Count() != 0)
        {
            yield return new WaitForSeconds(3f);
        }

        for (int metalArrayIndex = 0; metalArrayIndex < metalCherriesCountBySize.Length; metalArrayIndex++) // spawn one metal cherry for each indicated in array
        {
            for (int metalCherryCounter = 0; metalCherryCounter < metalCherriesCountBySize[metalArrayIndex]; metalCherryCounter++)
            {
                SpawnCherry((CherryTypes.CherrySize)metalArrayIndex, true);

                yield return new WaitForSeconds(timeBetweenCherries);
            }
        }

        int[] GetHowManyMetalCherries()
        {
            int[] metalCherryCountBySize = new int[4] { 0, 0, 0, 0 };
            while (totalCherries > mediumMetalCherryValue)
            {
                if (totalCherries > superLargeMetalCherryValue) { metalCherryCountBySize[3] += 1; totalCherries -= superLargeMetalCherryValue; }
                if (totalCherries > largeMetalCherryValue) { metalCherryCountBySize[2] += 1; totalCherries -= largeMetalCherryValue; }
                if (totalCherries > mediumMetalCherryValue) { metalCherryCountBySize[1] += 1; totalCherries -= mediumMetalCherryValue; }
            }
            return metalCherryCountBySize;
        }
    }

    private void SpawnCherry(CherryTypes.CherrySize size = CherryTypes.CherrySize.None, bool isMetal = false)
    {
        GameObject newCherry = Instantiate(cherryPrefab, cherryStartPos, Quaternion.identity);
        cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());

        if (size == CherryTypes.CherrySize.None)
        {
            SetSizeRandom(newCherry, bigChance);
        }
        else
        {
            SetSize(newCherry, size);
        }

        if (isMetal)
        {
            newCherry.GetComponent<CherryTypes>().IsMetal = true;
        }
    }

    IEnumerator HardCodedRoundCoroutine(HardCodedRound hardCodedRound)
    {
        RoundManager.roundManager.totalCherriesThisRound = hardCodedRound.cherryCount;

        for (int i = 0; i < hardCodedRound.cherryCount; i++)
        {
            SpawnCherry(hardCodedRound.GetCherrySize());

            yield return new WaitForSeconds(hardCodedRound.timeBetweenCherriesSeconds);
        }

        if (hardCodedRound.metalCherries.Count > 0) { yield return new WaitForSeconds(0.5f); }

        foreach (HardCodedRound.MetalCherry metalCherry in hardCodedRound.metalCherries)
        {
            for (int i = 0; i < metalCherry.sizeAndAmount.amount; i++)
            {
                SpawnCherry(metalCherry.sizeAndAmount.size, true);

                yield return new WaitForSeconds(hardCodedRound.timeBetweenCherriesSeconds);
            }
        }
    }

    private float CalculateAmountScaleFactor(float amountScaleValue)
    {
        return (Mathf.Pow(RoundManager.roundManager.roundNumber, amountScaleValue) / 2.5f) + 0.6f;
    }

    private float CalculateCherrySize(float sizeScaleValue)
    {
        return Mathf.Clamp(initialBigChance * Mathf.Pow(sizeScaleValue, RoundManager.roundManager.roundNumber - 1), 0, 100);
    }

    CherryTypes.CherrySize SetSizeRandom(GameObject cherry, float bigChance)
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
        
        return cherryTypes.cherrySize;
    }

    private void SetSize(GameObject cherry, CherryTypes.CherrySize size)
    {
        CherryTypes cherryTypes = cherry.GetComponent<CherryTypes>();

        cherryTypes.cherrySize = size;

        cherryTypes.SetCherryHealthAndSpeed();
    }

    [System.Serializable]
    public class RoundScalingMultipliers
    {
        public int startRoundNumber = 1;
        public float multiplier = 1;
    }
}
