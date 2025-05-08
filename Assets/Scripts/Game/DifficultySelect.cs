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
    [SerializeField] BatterSelect batterSelect;
    [SerializeField] TMPro.TextMeshProUGUI difficultyName;
    [SerializeField] List<Batter> batters;
    [SerializeField] LevelPreviewManager levelPreviewManager;
    [SerializeField] ParticleSystem fire;

    void Start()
    {
        difficulty = difficulties[0];
        UpdateDifficulty();
        if (!AchievementsTracker.Instance.HasCompletedLevel(levelPreviewManager.levelIndex, 3))
        {
            batters.RemoveAt(3);
        }
    }

    int difficultyIndex = 0;

    public void OnRightArrow()
    {
        difficultyIndex += 1;
        if (difficultyIndex >= batters.Count)
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
            difficultyIndex = batters.Count - 1;
        }

        UpdateDifficulty();
    }

    public void OnChangeBatter()
    {
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        difficulty = difficulties[difficultyIndex];
        difficulty.batter = GetActiveBatter();
        difficultyName.text = difficulties[difficultyIndex].name;
        if (difficultyIndex == 3) { fire.Play(); }
        else { fire.Stop(); }
        if (difficultyIndex != 0)
        {
            SoundEffectManager.sfxmanager.PlayOneShotWithPitch(cupFillSound, 1 + (0.25f * (difficultyIndex - 1)));
        }
        cup.sprite = cupSprites[difficultyIndex];
    }

    private Batter GetActiveBatter()
    {
        return batterSelect.GetActiveBatter();
    }
}


