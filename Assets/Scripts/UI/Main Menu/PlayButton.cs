using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{

    public Button playButton;
    public string sceneName;
    public int levelIndex = 0;

    void Start()
    {
        playButton.onClick.AddListener(PlayGame);
    }

    void PlayGame()
    {
        LevelManager.levelWasLoadedFromSave = false;
        TransitionManager.transitionManager.LoadScene(sceneName);
    }
}
