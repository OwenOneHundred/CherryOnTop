using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyInfo : MonoBehaviour
{
    [System.NonSerialized] public GameDifficultyParams gameDifficultyParams;
    public static DifficultyInfo difficultyInfo;
    [SerializeField] List<Sprite> measuringCupSprites = new();
    public int levelIndex = 0;
    [SerializeField] Difficulty defaultDifficulty;
    [SerializeField] Batter defaultBatter;
    void Awake()
    {
        gameDifficultyParams = new();
        if (difficultyInfo == this || difficultyInfo == null)
        {
            difficultyInfo = this;
            gameDifficultyParams.Batter = defaultBatter;
            gameDifficultyParams.Difficulty = defaultDifficulty;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SubscribeToLoadScene()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void UnsubscribeToLoadScene()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "LevelSelectScene" && scene.name != "MenuScene")
        {
            SetUpDifficulty();
            UnsubscribeToLoadScene();
        }
    }

    public void SetUpDifficulty(GameDifficultyParams incomingParams = null)
    {
        if (incomingParams != null)
        {
            if (incomingParams.Batter == null) { incomingParams.Batter = defaultBatter; }
            if (incomingParams.Difficulty == null) { incomingParams.Difficulty = defaultDifficulty; }
            gameDifficultyParams = incomingParams;
        }
        
        Image difficultyIcon = GameObject.Find("DifficultyIcon").GetComponent<Image>();
        difficultyIcon.sprite = measuringCupSprites[gameDifficultyParams.Difficulty.number - 1];

        gameDifficultyParams.Difficulty.OnGameStart();

        gameDifficultyParams.Batter.OnGameStart();
    }
}
