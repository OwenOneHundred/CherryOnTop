using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSaves;

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

        continueButton.gameObject.SetActive(SaveDataUtility.GetSaveFileNameIfExists(sceneNameInEditor, out string saveFilePath, out string saveFileName));

        SetUpRibbons(levelIndex);
    }

    private void SetUpRibbons(int levelIndex)
    {
        for (int i = 0; i < ribbons.Count; i++)
        {
            ribbons[i].SetActive(AchievementsTracker.Instance.HasCompletedLevel(levelIndex, i + 1));
        }
    }
}
