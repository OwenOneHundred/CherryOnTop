using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{

    public Button playButton;
    private static bool created = false;
    [SerializeField] private string sceneName;



    void Awake()
    {
        Debug.Log("Awake:" + SceneManager.GetActiveScene().name);

        // Ensure the script is not deleted while loading.
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }   
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
    {
        playButton.onClick.AddListener(PlayGame);
    }

    void PlayGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
