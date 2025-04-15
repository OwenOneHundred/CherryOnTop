using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyInfo : MonoBehaviour
{
    public float difficultyValue = 1.16f;
    public static DifficultyInfo difficultyInfo;
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
            FindAnyObjectByType<CherrySpawner>().difficultyScalingAmount = difficultyValue;
        }
    }
}
