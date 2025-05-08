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
    [SerializeField] GameObject batterLock;
    public int batterIndex = 0;

    public void Start()
    {
        if (!AchievementsTracker.Instance.HasCompletedLevel(levelPreviewManager.levelIndex, 4))
        {
            DisableBatterSelect();
        }
        OnChange();
    }

    public void OnRightArrow()
    {
        batterIndex += 1;
        if (batterIndex >= batters.Count)
        {
            batterIndex = 0;
        }

        OnChange();
    }

    public void OnLeftArrow()
    {
        batterIndex -= 1;
        if (batterIndex < 0)
        {
            batterIndex = batters.Count - 1;
        }

        OnChange();
    }

    public void OnChange()
    {
        Batter newBatter = batters[batterIndex];
        batterName.text = newBatter.visibleName;
        batterDescription.text = newBatter.description;
        batterBackground.color = newBatter.color;
        difficultySelect.OnChangeBatter();
    }

    public Batter GetActiveBatter()
    {
        return batters[batterIndex];
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

        batterLock.SetActive(true);
    }
}
