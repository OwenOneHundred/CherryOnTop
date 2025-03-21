using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public void QuitGameIfNotUnityEditor()
    {
        Application.Quit();
    }

    public void GotoMainMenu()
    {
        //SceneLoader.Instance.LoadMainMenu();
    }


}
