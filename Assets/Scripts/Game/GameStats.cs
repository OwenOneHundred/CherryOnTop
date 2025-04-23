using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStats : MonoBehaviour
{
    public static GameStats gameStats;

    public int toppingsBought = 0;
    public int toppingsSold = 0;
    public int toppingsPlaced = 0;

    public int moneyEarned = 0;
    public int moneySpent = 0;

    public int roundsCompleted = 0;
    public int cherriesKilled = 0;

    void Awake()
    {
        if (gameStats == this || gameStats == null)
        {
            gameStats = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
}
