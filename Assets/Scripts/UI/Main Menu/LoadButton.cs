using GameSaves;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour
{
    public Button playButton;
    public string _sceneName;
    public string _levelNameIngame;
    public string sceneName
    {
        get
        {
            return _sceneName;
        }
        set
        {
            _sceneName = value;
            //gameObject.SetActive(SaveDataUtility.GetSaveFileNameIfExists(_sceneName, out string saveFilePath, out string saveFileName));
        }
    }
    protected bool loadScene = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
    }

    void PlayGame()
    {
        SceneManager.sceneLoaded += LoadSceneData;
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneData(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= LoadSceneData;
        if (SaveDataUtility.GetSaveFileNameIfExists(scene.name, out string saveFilePath, out string saveFileName))
        {
            LevelManager.levelWasLoadedFromSave = true;
            LevelManager.Instance.Initialize(scene.name, true);
            LevelManager.Instance.LoadLevel();
        }
    }
}
