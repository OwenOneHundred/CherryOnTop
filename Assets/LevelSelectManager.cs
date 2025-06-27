using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public List<LevelPreview> levelPreviews;
    public LevelPreviewManager levelPreviewManager;

    int tab = 0;
    int currentDisplayedLevel = 0;
    [SerializeField] DotsButtonManager dotsButtonManager;

    [SerializeField] AudioFile switchLevelSound;

    [SerializeField] List<TabController> tabControllers;
    [SerializeField] List<Image> tabButtonImages;

    [SerializeField] DifficultySelect difficultySelect;
    [SerializeField] BatterSelect batterSelect;

    [SerializeField] Batter defaultBatter;
    [SerializeField] AudioFile switchTabs;

    void Start()
    {
        dotsButtonManager.SetUp(levelPreviews.Count, LoadLevel); // order of this and loadlevel(0) does matter

        LoadLevelSilent(0);
    }

    public void MoveRight()
    {
        currentDisplayedLevel += 1;
        if (currentDisplayedLevel >= levelPreviews.Count)
        {
            currentDisplayedLevel = 0;
        }
        if (currentDisplayedLevel < 0)
        {
            currentDisplayedLevel = levelPreviews.Count - 1;
        }
        LoadLevel(currentDisplayedLevel);
    }

    public void MoveLeft()
    {
        currentDisplayedLevel -= 1;
        if (currentDisplayedLevel >= levelPreviews.Count)
        {
            currentDisplayedLevel = 0;
        }
        if (currentDisplayedLevel < 0)
        {
            currentDisplayedLevel = levelPreviews.Count - 1;
        }
        LoadLevel(currentDisplayedLevel);
    }

    public void SwitchTabs(int tabIndex)
    {
        Color onColor = new Color(0.8313726f, 0.6941177f, 0.7529f, 1);
        Color offColor = new Color(0.6431373f, 0.58f, 0.58f, 1);

        tabControllers[tab].OnSwitchedOff();
        tabButtonImages[tab].color = offColor;

        tabControllers[tabIndex].OnSwitchedTo();
        tabButtonImages[tabIndex].color = onColor;

        if (tab != tabIndex) { SoundEffectManager.sfxmanager.PlayOneShot(switchTabs); }
        tab = tabIndex;

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { MoveLeft(); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { MoveRight(); }
    }

    public void LoadLevel(int index)
    {
        LevelPreview currentPreview = levelPreviews[index];
        levelPreviewManager.Setup(currentPreview.levelImage, currentPreview.sceneNameIngame, currentPreview.sceneNameInEditor, index);

        dotsButtonManager.SetActiveDot(index);

        SoundEffectManager.sfxmanager.PlayOneShot(switchLevelSound);

        currentDisplayedLevel = index;
    }

    private void LoadLevelSilent(int index)
    {
        LevelPreview currentPreview = levelPreviews[index];
        levelPreviewManager.Setup(currentPreview.levelImage, currentPreview.sceneNameIngame, currentPreview.sceneNameInEditor, index);

        dotsButtonManager.SetActiveDot(index);
    }

    public void OnPressedNewGame() // called when new game button pressed
    {
        DifficultyInfo.difficultyInfo.SubscribeToLoadScene();
        DifficultyInfo.difficultyInfo.gameDifficultyParams.Batter = GetBatter();
        DifficultyInfo.difficultyInfo.gameDifficultyParams.Difficulty = GetDifficulty();
        DifficultyInfo.difficultyInfo.levelIndex = currentDisplayedLevel;

        LevelManager.levelWasLoadedFromSave = false;
        TransitionManager.transitionManager.LoadScene(levelPreviewManager.sceneNameInEditor);
    }

    public Batter GetBatter() // called on start new game to get the active batter
    {
        if (tab == 0)
        {
            return defaultBatter;
        }
        else if (tab == 1)
        {
            return batterSelect.GetBatter();
        }
        else
        {
            throw new System.InvalidOperationException("Started new game on unsupported tab: " + tab);
        }
    }

    public Difficulty GetDifficulty()
    {
        if (tab == 0)
        {
            return difficultySelect.GetDifficulty();
        }
        else if (tab == 1)
        {
            return batterSelect.GetDifficulty();
        }
        else
        {
            throw new System.InvalidOperationException("Started new game on unsupported tab: " + tab);
        }
    }
}
