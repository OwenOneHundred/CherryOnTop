using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public void QuitGameIfNotUnityEditor()
    {
        Application.Quit();
    }

    public void GotoMainMenu()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SaveLevel();
        }
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }


}
