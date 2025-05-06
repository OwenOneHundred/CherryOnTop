using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI moneyText;
    [SerializeField] TMPro.TextMeshProUGUI cakePointsText;
    [SerializeField] TMPro.TextMeshProUGUI roundNumberText;
    [SerializeField] TMPro.TextMeshProUGUI goalRoundText;
    [SerializeField] TMPro.TextMeshProUGUI goalScoreText;
    [SerializeField] AudioFile speedUpSound;
    [SerializeField] AudioFile slowDownSound;
    [SerializeField] SettingsManager settingsManager;

    void Start()
    {
        settingsManager.OnStart();
    }

    public void SetMoney(int money)
    {
        //moneyText.text = "Money: $" + money;
        moneyText.text = "$" + money;
    }

    public void SetCakePoints(int cakePoints)
    {
        //cakePointsText.text = "Cake Points: " + cakePoints;
        cakePointsText.text = "" + cakePoints;
    }

    public void SetRound(uint roundNumber)
    {
        roundNumberText.text = "Round " + roundNumber;
    }

    public void SetCakeScoreGoal(int goal, int roundNumber)
    {
        goalScoreText.text = "Next goal: " + goal;
        goalRoundText.text = "by round " + roundNumber;
    }

    bool speedupToggled = false; 
    [SerializeField] Color speedUpButtonUntoggled;
    [SerializeField] Color speedUpButtonToggled;
    [SerializeField] Image speedUpButton;
    [SerializeField] TMPro.TextMeshProUGUI speedUpButtonText;
    public void PressSpeedUpButton()
    {
        if (speedupToggled)
        {
            Time.timeScale = 1f;
            SoundEffectManager.sfxmanager.PlayOneShot(slowDownSound);
        }
        else
        {
            Time.timeScale = 2f;
            SoundEffectManager.sfxmanager.PlayOneShot(speedUpSound);
        }
        speedupToggled = !speedupToggled;
        speedUpButton.color = speedupToggled ? speedUpButtonToggled : speedUpButtonUntoggled;
        speedUpButtonText.text = speedupToggled ? "2x" : "1x";
    }
}
