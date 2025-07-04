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

    void Awake()
    {
        startButtonLockImage = startButtonLock.GetComponent<Image>();
    }

    void Start()
    {
        settingsManager.OnStart();

        if (!PlayerPrefs.HasKey("TutorialFinished") || DEBUG_ForceTutorial)
        {
            Instantiate(tutorial);
        }
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

    bool _playLockDisabled = false;
    public bool PlayLockDisabled
    {
        get { return _playLockDisabled; }
        set
        {
            _playLockDisabled = value;
            startButtonLockImage.GetComponent<Button>().interactable = !value;
        }
    }

    bool _speedUpButtonLocked = false;
    public bool SpeedUpButtonLocked
    {
        get { return _speedUpButtonLocked; }
        set
        {
            _speedUpButtonLocked = value;
            speedUpButton.GetComponent<Button>().interactable = !value;
        }
    }

    bool _speedupToggled = false;
    public bool SpeedUpToggled => _speedupToggled;
    [SerializeField] Color speedUpButtonUntoggled;
    [SerializeField] Color speedUpButtonToggled;
    [SerializeField] Image speedUpButton;
    [SerializeField] TMPro.TextMeshProUGUI speedUpButtonText;
    public void PressSpeedUpButton()
    {
        SetSpeedUp(!SpeedUpToggled, false);
    }

    public void SetSpeedUp(bool speedUp, bool silent)
    {
        if (_speedupToggled == speedUp) { return; }

        if (!silent) { SoundEffectManager.sfxmanager.PlayOneShot(speedUpButtonClickSound); }
        if (speedUp)
        {
            Time.timeScale = 2f;
            if (!silent) { SoundEffectManager.sfxmanager.PlayOneShot(speedUpSound); }
        }
        else
        {
            Time.timeScale = 1f;
            if (!silent) { SoundEffectManager.sfxmanager.PlayOneShot(slowDownSound); }
        }
        _speedupToggled = speedUp;
        speedUpButton.color = speedUp ? speedUpButtonToggled : speedUpButtonUntoggled;
        speedUpButtonText.text = speedUp ? "2x" : "1x";
    }

    public void OnLockClicked()
    {
        StartIsLocked = !StartIsLocked;

        SetLockValue(StartIsLocked, false);
    }

    public void SetLockValue(bool locked, bool silent)
    {
        StartIsLocked = locked;

        if (StartIsLocked)
        {
            startButtonLockImage.sprite = lockedLock;
            if (!silent) { SoundEffectManager.sfxmanager.PlayOneShotWithPitch(lockSound, 0.75f); }
            startButtonLockImage.color = Color.white;
        }
        else
        {
            startButtonLockImage.sprite = unlockedLock;
            if (!silent) { SoundEffectManager.sfxmanager.PlayOneShotWithPitch(lockSound, 1.25f); }
            startButtonLockImage.color = new Color(0.65f, 0.65f, 0.65f, 1);
        }
    }
}
