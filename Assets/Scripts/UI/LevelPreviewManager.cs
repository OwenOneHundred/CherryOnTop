using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSaves;
using System.Linq;

public class LevelPreviewManager : MonoBehaviour
{
    [SerializeField] List<Image> previewImages;
    [SerializeField] TMPro.TextMeshProUGUI title;
    [SerializeField] List<GameObject> ribbons;
    public string sceneNameInEditor;

    [SerializeField] LoadButton continueButton;

    public int levelIndex = 0;

    public void Setup(Sprite levelImage, string sceneNameInGame, string sceneNameInEditor, int levelIndex)
    {
        foreach (Image image in previewImages) { image.sprite = levelImage; }
        this.sceneNameInEditor = sceneNameInEditor;
        this.levelIndex = levelIndex;
        continueButton.sceneName = sceneNameInEditor;
        continueButton._levelNameIngame = sceneNameInGame;
        title.text = sceneNameInGame;

        if (SaveDataUtility.GetSaveFileNameIfExists(sceneNameInEditor, out string saveFilePath, out string saveFileName))
        {
            continueButton.gameObject.SetActive(true);

            SetContinueButtonPreviewData(saveFileName);
        }
        else
        {
            continueButton.gameObject.SetActive(false);
        }

        SetUpRibbons(levelIndex);
    }

    private void SetContinueButtonPreviewData(string saveFileName)
    {
        SaveData saveData = SaveDataUtility.LoadSaveData(CutExtensionOffOfFileName(saveFileName), sceneNameInEditor);

        uint roundNumber = 0;
        int moneyNumber = 0;
        int batterIndex = -1;
        if (saveData.TryGetDataEntry("round", out DEUIntEntry round))
        {
            roundNumber = round.value;
        }

        if (saveData.TryGetDataEntry("money", out DEIntEntry money))
        {
            moneyNumber = money.value;
        }

        if (saveData.TryGetDataEntry("batter", out DEIntEntry batter))
        {
            batterIndex = batter.value;
        }

        continueButton.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text =
            "Round " + roundNumber +
            ", $" + moneyNumber;
        continueButton.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text =
            batterIndex == -1 ? "" : ((Constants.Batter)batterIndex).ToString().Replace('_', ' ');
    }

    private string CutExtensionOffOfFileName(string fileName)
    {
        return fileName.Substring(0, fileName.Length - 4);
    }

    private void SetUpRibbons(int levelIndex)
    {
        for (int i = 0; i < ribbons.Count; i++)
        {
            ribbons[i].SetActive(AchievementsTracker.Instance.HasCompletedLevel(levelIndex, i + 1, -1));
        }
    }
}
