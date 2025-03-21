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
        SceneManager.LoadScene("MenuScene");
    }


}
