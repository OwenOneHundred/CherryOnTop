using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultyInfo : MonoBehaviour
{
    public DifficultySelect.Difficulty difficulty;
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
            OnLoadedGameScene();
        }
    }

    private void OnLoadedGameScene()
    {
        FindAnyObjectByType<CherrySpawner>().difficultyScalingAmount = difficulty.value;

        Transform icons = GameObject.Find("DifficultyIcons").transform;
        int budgetEnum = 0;
        foreach (Transform trans in icons)
        {
            if (budgetEnum >= difficulty.number)
            {
                trans.GetComponent<Image>().color = Color.gray;
            }
            else
            {
                trans.GetComponent<Image>().color = Color.white;
            }

            budgetEnum += 1;
        }

        difficulty.OnRoundStart();
    }
}
