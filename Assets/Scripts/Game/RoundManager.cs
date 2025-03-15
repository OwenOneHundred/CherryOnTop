using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public RoundState roundState;
    public int roundNumber = 0;

    [System.NonSerialized] public int totalCherriesThisRound = 10;
    int cherriesKilledThisRoundCount = 0;
    [SerializeField] int moneyOnRoundEnd = 10;

    public static RoundManager roundManager; // Singleton
    [SerializeField] Button nextRoundButton;
    [SerializeField] IngameUI ingameUI;

    void Awake()
    {
        if (roundManager == null)
        {
            roundManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartNextRound() // called by start round button
    {
        roundNumber += 1;
        ingameUI.SetRound(roundNumber);
        roundState = RoundState.cherries;
        nextRoundButton.interactable = false;
        GameObject.FindAnyObjectByType<CherrySpawner>().OnRoundStart();
    }

    public void EndRound() // called by OnCherryKilled when last cherry dies
    {
        roundState = RoundState.shop;
        nextRoundButton.interactable = true;
        Inventory.inventory.Money += moneyOnRoundEnd;
        cherriesKilledThisRoundCount = 0;
    }

    public void OnCherryKilled()
    {
        cherriesKilledThisRoundCount += 1;
        if (cherriesKilledThisRoundCount >= totalCherriesThisRound)
        {
            EndRound();
        }
    }

    public enum RoundState
    {
        none, cherries, shop 
    }
}
