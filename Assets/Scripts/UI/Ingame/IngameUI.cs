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
    [SerializeField] GameObject tutorial;
    [SerializeField] Button startButtonLock;
    [SerializeField] AudioFile speedUpButtonClickSound;
    Image startButtonLockImage;
    [SerializeField] Sprite lockedLock;
    [SerializeField] Sprite unlockedLock;
    [SerializeField] AudioFile lockSound;
    public bool StartIsLocked = false;
    [SerializeField] bool DEBUG_ForceTutorial = false;

    void Start()
    {
        settingsManager.OnStart();

        if (!PlayerPrefs.HasKey("TutorialFinished") || DEBUG_ForceTutorial)
        {
            Instantiate(tutorial);
        }

        startButtonLockImage = startButtonLock.GetComponent<Image>();
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
        SoundEffectManager.sfxmanager.PlayOneShot(speedUpButtonClickSound);
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

    public void OnLockClicked()
    {
        StartIsLocked = !StartIsLocked;

        if (StartIsLocked)
        {
            startButtonLockImage.sprite = lockedLock;
            SoundEffectManager.sfxmanager.PlayOneShotWithPitch(lockSound, 0.75f);
            startButtonLockImage.color = Color.white;
        }
        else
        {
            startButtonLockImage.sprite = unlockedLock;
            SoundEffectManager.sfxmanager.PlayOneShotWithPitch(lockSound, 1.25f);
            startButtonLockImage.color = new Color(0.65f, 0.65f, 0.65f, 1);
        }
    }

    public void DisableLock()
    {
        startButtonLock.interactable = false;
    }

    public void EnableLock()
    {
        startButtonLock.interactable = true;
    }


}
