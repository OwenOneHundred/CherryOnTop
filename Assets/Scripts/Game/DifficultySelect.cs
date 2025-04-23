using System.Collections.Generic;
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

    void Start()
    {
        difficulty = difficulties[0];
    }

    int difficultyIndex = 0;

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
        difficulty = difficulties[difficultyIndex];
        difficultyName.text = difficulties[difficultyIndex].name;

        cup.sprite = cupSprites[difficultyIndex];
    }
}


