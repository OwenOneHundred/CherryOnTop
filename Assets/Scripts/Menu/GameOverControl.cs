using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverControl : MonoBehaviour
{
    public static GameOverControl gameOverControl;
    private void Awake()
    {
        if (gameOverControl == null)
        {
            gameOverControl = this;
        }
        else if (gameOverControl != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void OnGameOver()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1;
    }
}