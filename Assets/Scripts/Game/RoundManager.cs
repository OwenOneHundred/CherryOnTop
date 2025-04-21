using EventBus;
using GameSaves;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(LevelManager))]
public class RoundManager : MonoBehaviour
{
    public RoundState roundState = RoundState.shop;
    public uint roundNumber = 0;

    [System.NonSerialized] public int totalCherriesThisRound = 10;
    int cherriesKilledThisRoundCount = 0;
    public int moneyOnRoundEnd = 10;

    public static RoundManager roundManager; // Singleton
    [SerializeField] Button nextRoundButton;
    [SerializeField] Button shopButton;
    [SerializeField] IngameUI ingameUI;
    [SerializeField] Shop shop;
    [SerializeField] bool savingEnabled = true;

    LevelManager _levelManager;
    LevelManager levelManager
    {
        get
        {
            if (_levelManager == null)
                _levelManager = GetComponent<LevelManager>();
            return _levelManager;
        }
    }

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
        roundState = RoundState.shop;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && roundState == RoundState.shop)
        {
            StartNextRound();
        }
    }

    public void InitializeSave(bool loadLevel = false)
    {
        levelManager.Initialize(SceneManager.GetActiveScene().name, loadLevel);
    }

    public void SaveLevel()
    {
        InitializeSave();
        levelManager.SaveLevel();
    }

    public void StartNextRound() // called by start round button
    {
        if (savingEnabled)
        {
            SaveLevel(); // save before round start things are finished
        }

        roundNumber += 1;
        ingameUI.SetRound(roundNumber);
        roundState = RoundState.cherries;
        nextRoundButton.interactable = false;
        shopButton.interactable = false;
        cherriesKilledThisRoundCount = 0;
        if (shop.Open) shop.ToggleOpen();

        EventBus<RoundStartEvent>.Raise(new RoundStartEvent(roundNumber));

        GameObject.FindAnyObjectByType<CherrySpawner>().OnRoundStart();
        shop.OnRoundEnd();
    }

    public void EndRound() // called by OnCherryKilled when last cherry dies
    {
        roundState = RoundState.shop;
        nextRoundButton.interactable = true;
        shopButton.interactable = true;
        Inventory.inventory.Money += moneyOnRoundEnd;
        cherriesKilledThisRoundCount = 0;

        EventBus<RoundEndEvent>.Raise(new RoundEndEvent()); // the order of this and OnRoundEnd DOES matter
        foreach (ToppingRegistry.ItemInfo itemInfo in ToppingRegistry.toppingRegistry.PlacedToppings)
        {
            itemInfo.topping.OnRoundEnd();
        }

        CakePointsManager.cakePointsManager.OnRoundEnd();

        if (savingEnabled)
        {
            SaveLevel(); // save after all round end things are called
        }   
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
