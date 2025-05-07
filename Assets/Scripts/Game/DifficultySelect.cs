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
    [SerializeField] List<Batter> batters;
    [SerializeField] TMP_Dropdown dropdown;
    [SerializeField] TMPro.TextMeshProUGUI batterDescription;
    [SerializeField] GameObject batterDescriptionObj;
    [SerializeField] Image batterBackground;

    void Start()
    {
        difficulty = difficulties[0];
        UpdateDifficulty();
    }

    int difficultyIndex = 0;

    void Update()
    {
        batterDescriptionObj.SetActive(!dropdown.IsExpanded && dropdown.value != 0);
    }

    // add code disabling description in update if dropdown.isExpanded, and update description in ChangeBatter

    public void Click()
    {
        difficultyIndex += 1;
        if (difficultyIndex >= difficulties.Count)
        {
            difficultyIndex = 0;
        }
        else
        {
            SoundEffectManager.sfxmanager.PlayOneShotWithPitch(cupFillSound, 1 + (0.25f * (difficultyIndex - 1)));
        }
        
        UpdateDifficulty();

        cup.sprite = cupSprites[difficultyIndex];
    }

    public void ChangeBatter()
    {
        UpdateDifficulty();
    }

    private void UpdateDifficulty()
    {
        difficulty = difficulties[difficultyIndex];
        difficulty.batter = GetActiveBatter();
        difficultyName.text = difficulties[difficultyIndex].name;
        batterDescription.text = difficulty.batter.description;
        batterBackground.color = difficulty.batter.color;
    }

    private Batter GetActiveBatter()
    {
        return batters[dropdown.value];
    }
}


