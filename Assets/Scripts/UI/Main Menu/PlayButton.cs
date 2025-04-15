using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{

    public Button playButton;
    public string sceneName;
    public bool alsoGetDifficulty = true;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            DifficultyInfo.difficultyInfo.difficultyValue = GetDifficultyValue();
        }
    }

    private float GetDifficultyValue()
    {
        return transform.root.GetComponentInChildren<DifficultySelect>().difficultyValue;
    }
}
