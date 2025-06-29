using System.Collections.Generic;
using GameSaves;
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
    [SerializeField] List<StateLockableButton> stateLockableButtons = new();
    public int batterIndex = -1;

    public void Start()
    {
        SetUp();

        ChangeBatter(0, true);

        UpdateBatterCompletions();
    }

    private void SetUp()
    {
        if (stateLockableButtons.Count != 0) { return; }

        for (int i = 0; i < batters.Count; i++)
        {
            batters[i].index = i;

            int index = i;
            GameObject batterButton = Instantiate(batterButtonPrefab, batterListParent);
            batterButton.GetComponent<Button>().onClick.AddListener(() => ChangeBatter(index));
            batterButton.GetComponentInChildren<TextMeshProUGUI>().text = batters[i].visibleName;

            stateLockableButtons.Add(batterButton.GetComponent<StateLockableButton>());
        }
    }

    public void UpdateBatterCompletions()
    {
        if (stateLockableButtons.Count == 0) { SetUp(); }

        for (int i = 0; i < batters.Count; i++)
        {
            if (AchievementsTracker.Instance.HasCompletedLevel(levelPreviewManager.levelIndex, 0, i))
            {
                stateLockableButtons[i].transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                stateLockableButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void ChangeBatter(int index, bool silent = false)
    {
        if (index == batterIndex) { return; }

        batterIndex = index;
        Batter newBatter = batters[batterIndex];
        batterName.text = newBatter.visibleName;
        batterDescription.text = newBatter.description;

        for (int i = 0; i < stateLockableButtons.Count; i++)
        {
            stateLockableButtons[i].stateLocks.Clear();
            if (i != index)
            {
                stateLockableButtons[i].TransitionToNormalState();
            }
            else
            {
                stateLockableButtons[index].TransitionToSelectedState();
                stateLockableButtons[index].stateLocks.Add(new StateLockableButton.StateLock(StateLockableButton.StateLock.LockType.everything, ""));
            }
        }

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
