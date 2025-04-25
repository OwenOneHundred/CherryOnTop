using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyInfo : MonoBehaviour
{
    [System.NonSerialized] public Difficulty difficulty;
    public static DifficultyInfo difficultyInfo;
    [SerializeField] List<Sprite> measuringCupSprites = new();
    public int levelIndex = 0;
    [SerializeField] Difficulty defaultDifficulty;
    void Awake()
    {
        if (difficultyInfo == this || difficultyInfo == null)
        {
            difficultyInfo = this;
            difficulty = defaultDifficulty;
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
            LoadDifficulty(difficulty);
            UnsubscribeToLoadScene();
        }
    }

    public void LoadDifficulty(Difficulty difficulty = null)
    {
        if (difficulty == null) difficulty = this.difficulty;
        else this.difficulty = difficulty;
        FindAnyObjectByType<CherrySpawner>().difficulty = difficulty;

        Image difficultyIcon = GameObject.Find("DifficultyIcon").GetComponent<Image>();
        difficultyIcon.sprite = measuringCupSprites[difficulty.number - 1];

        difficulty.OnRoundStart();
    }
}
