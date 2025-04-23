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
    void Awake()
    {
        if (difficultyInfo == this || difficultyInfo == null)
        {
            difficultyInfo = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name != "LevelSelectScene" && scene.name != "MenuScene")
        {
            OnLoadedGameScene();
        }
    }

    private void OnLoadedGameScene()
    {
        FindAnyObjectByType<CherrySpawner>().difficulty = difficulty;

        Image difficultyIcon = GameObject.Find("DifficultyIcon").GetComponent<Image>();
        difficultyIcon.sprite = measuringCupSprites[difficulty.number - 1];

        difficulty.OnRoundStart();
    }
}
