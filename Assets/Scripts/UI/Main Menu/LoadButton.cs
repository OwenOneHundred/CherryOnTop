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

    void PlayGame() // called when load game button pressed
    {
        SceneManager.sceneLoaded += LoadSceneData;
        TransitionManager.transitionManager.LoadScene(sceneName);
    }

    public void LoadSceneData(Scene scene, LoadSceneMode loadSceneMode) // called when new scene is loaded for load game
    {
        SceneManager.sceneLoaded -= LoadSceneData;
        if (SaveDataUtility.GetSaveFileNameIfExists(scene.name, out string saveFilePath, out string saveFileName))
        {
            LevelManager.levelWasLoadedFromSave = true;
            LevelManager.Instance.Initialize(scene.name, true);
            LevelManager.Instance.LoadLevel();
            GameDifficultyParams gameDifficultyParams = new();
            if (LevelManager.Instance.saveData.TryGetDataEntry("difficulty", out DEInt2Entry difficulty))
            {
                gameDifficultyParams.Difficulty = LevelManager.Instance.DifficultyList.Find(d => d.number == difficulty.value1);
            }
            if (LevelManager.Instance.saveData.TryGetDataEntry("batter", out DEInt2Entry batter))
            {
                gameDifficultyParams.Batter = LevelManager.Instance.BatterList.Find(b => b.index == batter.value1);
            }

            DifficultyInfo.difficultyInfo.SetUpDifficulty(gameDifficultyParams);
        } else
        {
            DifficultyInfo.difficultyInfo.SetUpDifficulty();
        }
    }
}
