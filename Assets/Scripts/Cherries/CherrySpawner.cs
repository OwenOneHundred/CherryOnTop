using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        cherryManager = GetComponent<CherryManager>();
    }
    
    public void OnRoundStart()
    {
        if (difficulty == null) { difficulty = DifficultyInfo.difficultyInfo.gameDifficultyParams.Difficulty; }
        if (difficulty.TryGetHardCodedRound((int) RoundManager.roundManager.roundNumber, out HardCodedRound hardCodedRound))
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

        for (int i = 0; i < totalCherries; i++)
        {
            GameObject newCherry = Instantiate(cherryPrefab, cherryStartPos, Quaternion.identity);
            cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());
            SetSizeRandom(newCherry, bigChance);

            yield return new WaitForSeconds(timeBetweenCherries);
        }
    }

    IEnumerator HardCodedRoundCoroutine(HardCodedRound hardCodedRound)
    {
        RoundManager.roundManager.totalCherriesThisRound = hardCodedRound.cherryCount;

        for (int i = 0; i < hardCodedRound.cherryCount; i++)
        {
            SpecialtyCherry specialtyCherry = hardCodedRound.GetCherryPrefab();
            GameObject prefab;
            if (specialtyCherry.prefab == null) { prefab = cherryPrefab; }
            else { prefab = specialtyCherry.prefab; }

            GameObject newCherry = Instantiate(prefab, cherryStartPos, Quaternion.identity);
            cherryManager.RegisterCherry(newCherry.GetComponentInChildren<CherryMovement>());
            SetSize(newCherry, hardCodedRound.GetCherrySize());

            yield return new WaitForSeconds(hardCodedRound.timeBetweenCherriesSeconds);
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

    void SetSizeRandom(GameObject cherry, float bigChance)
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
