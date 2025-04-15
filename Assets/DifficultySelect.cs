using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    [SerializeField] List<Image> images;
    public float difficultyValue = 1.16f;

    public void PressEasy()
    {
        images[1].color = Color.gray;
        images[2].color = Color.gray;
        difficultyValue = 1.16f;
    }

    public void PressMedium()
    {
        images[1].color = Color.white;
        images[2].color = Color.gray;
        difficultyValue = 1.25f;
    }

    public void PressHard()
    {
        images[1].color = Color.white;
        images[2].color = Color.white;
        difficultyValue = 1.36f;
    }
}
