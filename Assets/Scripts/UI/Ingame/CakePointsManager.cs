using System.Linq;
using UnityEngine;

public class CakePointsManager : MonoBehaviour
{
    IngameUI ingameUI;

    int cakePoints = 0;
    public int CakePoints
    {
        get { return cakePoints; }
        set
        {
            cakePoints = value;
            ingameUI.SetCakePoints(value);
        }
    }
    [SerializeField] float cakePointsThresholdDivisor = 2.0f;
    [SerializeField] int howOftenThreshold = 10;
    [SerializeField] float scalePower = 3;
    [SerializeField] float b = 20;
    public static CakePointsManager cakePointsManager;

    void Awake()
    {
        ingameUI = FindAnyObjectByType<IngameUI>();

        if (cakePointsManager == this || cakePointsManager == null) { cakePointsManager = this; }
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        SetUpNextThreshold();
    }

    public void OnRoundEnd()
    {
        AddCakePointsForRound();
        if (RoundManager.roundManager.roundNumber == GetNextThresholdRoundNumber())
        {
            if (CheckIfReachedCakeThreshold())
            {
                OnMadeThreshold();
            }
            else
            {
                OnFailedThreshold();
            }
        }
    }

    public bool CheckIfReachedCakeThreshold()
    {
        return CakePoints >= GetNextCakePointsThreshold();
    }

    public int GetNextCakePointsThreshold()
    {
        int roundNumberToCheck = GetNextThresholdRoundNumber();
        return Mathf.FloorToInt(b + (Mathf.Pow(roundNumberToCheck, scalePower) / cakePointsThresholdDivisor));
    }

    public int GetNextThresholdRoundNumber()
    {
        int rawThreshold = Mathf.CeilToInt(RoundManager.roundManager.roundNumber / howOftenThreshold) * howOftenThreshold;
        if (rawThreshold == 0) { rawThreshold = howOftenThreshold; }
        return rawThreshold;
    }

    private void SetUpNextThreshold()
    {
        ingameUI.SetCakeScoreGoal(GetNextCakePointsThreshold(), GetNextThresholdRoundNumber());
    }

    public void OnMadeThreshold()
    {
        SetUpNextThreshold();
        Debug.Log("Made threshold!");
    }

    public void OnFailedThreshold()
    {
        Debug.Log("Failed threshold!");
    }

    public void AddCakePointsForRound()
    {
        foreach (Topping topping in ToppingRegistry.toppingRegistry.GetAllPlacedToppings().Select(x => x.topping))
        {
            CakePoints += topping.cakePoints;
        }
    }
}
