using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI moneyText;
    [SerializeField] TMPro.TextMeshProUGUI cakeScoreText;
    [SerializeField] TMPro.TextMeshProUGUI roundNumberText;
    [SerializeField] TMPro.TextMeshProUGUI goalRoundText;
    [SerializeField] TMPro.TextMeshProUGUI goalScoreText;
    public void SetMoney(int money)
    {
        moneyText.text = "Money: $" + money;
    }

    public void SetCakeScore(int cakeScore)
    {
        cakeScoreText.text = "Cake Score: " + cakeScore;
    }

    public void SetRound(int roundNumber)
    {
        roundNumberText.text = "Round " + roundNumber;
    }

    public void SetCakeScoreGoal(int goal, int roundNumber)
    {
        goalScoreText.text = "Next goal: " + goal;
        goalRoundText.text = "by round " + roundNumber;
    }
}
