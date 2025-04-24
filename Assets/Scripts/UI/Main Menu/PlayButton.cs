using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{

    public Button playButton;
    public string sceneName;
    public bool alsoGetDifficulty = true;
    public int levelIndex = 0;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
    }

    void PlayGame()
    {
        LevelManager.levelWasLoadedFromSave = false;
        SceneManager.LoadScene(sceneName);

        if (alsoGetDifficulty)
        {
            DifficultyInfo.difficultyInfo.SubscribeToLoadScene();
            DifficultyInfo.difficultyInfo.difficulty = GetDifficultyValue();
            DifficultyInfo.difficultyInfo.levelIndex = levelIndex;
        }
    }

    private Difficulty GetDifficultyValue()
    {
        return transform.root.GetComponentInChildren<DifficultySelect>().difficulty;
    }
}
