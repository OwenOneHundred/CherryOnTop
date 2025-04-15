using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyInMenu : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        if (scene.name != "LevelSelectScene" && scene.name != "MenuScene")
        {
            Destroy(gameObject);
        }
    }
}
