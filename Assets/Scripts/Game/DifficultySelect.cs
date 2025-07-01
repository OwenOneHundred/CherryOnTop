using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField] Image cup;
    [System.NonSerialized] public Difficulty difficulty;
    [SerializeField] List<Difficulty> difficulties;
    public List<Sprite> cupSprites;
    [SerializeField] AudioFile cupFillSound;
    [SerializeField] TMPro.TextMeshProUGUI difficultyName;
    [SerializeField] LevelPreviewManager levelPreviewManager;
    [SerializeField] ParticleSystem fire;

    void Start()
    {
        difficulty = difficulties[0];
        UpdateDifficulty(true);
        if (!AchievementsTracker.Instance.HasCompletedLevel(levelPreviewManager.levelIndex, 3, (int) Constants.Batter.None))
        {
            difficulties.RemoveAt(3);
        }
    }

    int difficultyIndex = 0;

    public void OnRightArrow()
    {
        difficultyIndex += 1;
        if (difficultyIndex >= difficulties.Count)
        {
            difficultyIndex = 0;
        }

        UpdateDifficulty();
    }

    public void OnLeftArrow()
    {
        difficultyIndex -= 1;
        if (difficultyIndex < 0)
        {
            difficultyIndex = difficulties.Count - 1;
        }

        UpdateDifficulty();
    }

    public void SetDifficulty(int difficultyIndex, bool silent = false)
    {
        if (this.difficultyIndex == difficultyIndex) { return; }

        this.difficultyIndex = difficultyIndex;

        UpdateDifficulty(silent);
    }

    private void UpdateDifficulty(bool silent = false)
    {
        difficulty = difficulties[difficultyIndex];
        difficultyName.text = difficulties[difficultyIndex].name;
        if (difficultyIndex == 3) { fire.Play(); }
        else { fire.Stop(); }
        if (!silent)
        {
            SoundEffectManager.sfxmanager.PlayOneShotWithPitch(cupFillSound, 0.75f + (0.25f * difficultyIndex));
        }
        cup.sprite = cupSprites[difficultyIndex];
    }

    public Difficulty GetDifficulty()
    {
        return difficulty;
    }
}


