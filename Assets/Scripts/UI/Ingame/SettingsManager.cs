using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void QuitGameIfNotUnityEditor()
    {
        Application.Quit();
    }

    public void GotoMainMenu()
    {
        if (LevelManager.Instance != null && LevelManager.Instance.roundManager.roundState != RoundManager.RoundState.cherries)
        {
            LevelManager.Instance.SaveLevel();
        }
        Time.timeScale = 1;
        TransitionManager.transitionManager.LoadScene("MenuScene");
    }


}
