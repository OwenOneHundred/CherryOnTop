using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BatterSelect : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI batterName;
    [SerializeField] List<Batter> batters;
    [SerializeField] TMPro.TextMeshProUGUI batterDescription;
    [SerializeField] GameObject batterDescriptionObj;
    [SerializeField] Image batterBackground;
    [SerializeField] DifficultySelect difficultySelect;
    [SerializeField] LevelPreviewManager levelPreviewManager;
    [SerializeField] Transform batterListParent;
    [SerializeField] GameObject batterButtonPrefab;
    public int batterIndex = 0;

    public void Start()
    {
        if (!AchievementsTracker.Instance.HasCompletedLevel(levelPreviewManager.levelIndex, 4))
        {
            //DisableBatterSelect();
        }

        SetUp();

        ChangeBatter(0, true);
    }

    private void SetUp()
    {
        for (int i = 0; i < batters.Count; i++)
        {
            int index = i;
            GameObject batterButton = Instantiate(batterButtonPrefab, batterListParent);
            batterButton.GetComponent<Button>().onClick.AddListener(() =>
                ChangeBatter(index)
            );
            batterButton.GetComponentInChildren<TextMeshProUGUI>().text = batters[i].visibleName;
        }
    }

    public void ChangeBatter(int index, bool silent = false)
    {
        batterIndex = index;
        Batter newBatter = batters[batterIndex];
        batterName.text = newBatter.visibleName;
        batterDescription.text = newBatter.description;

        difficultySelect.SetDifficulty(newBatter.associatedDifficulty.number - 1, silent);
    }

    public Batter GetBatter()
    {
        return batters[batterIndex];
    }

    public Difficulty GetDifficulty()
    {
        return batters[batterIndex].associatedDifficulty;
    }

    private void DisableBatterSelect()
    {
        foreach (Transform trans in transform)
        {
            if (trans.TryGetComponent<Button>(out Button button))
            {
                button.interactable = false;
            }
            else if (trans.TryGetComponent<Image>(out Image image))
            {
                Color oldColor = image.color;
                oldColor *= 0.5f;
                oldColor.a *= 0.5f;
                image.color = oldColor;
            }
            else if (trans.TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
            {
                Color oldColor = text.color;
                oldColor *= 0.5f;
                oldColor.a *= 0.7f;
                text.color = oldColor;
            }
        }
    }
}
