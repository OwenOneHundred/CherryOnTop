using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameSaves;

public class LevelPreviewManager : MonoBehaviour
{
    [SerializeField] Image previewImage;
    [SerializeField] PlayButton playButton;
    [SerializeField] TMPro.TextMeshProUGUI title;
    [SerializeField] List<GameObject> ribbons;
    [SerializeField] GameObject continueButtonObject;
    [SerializeField] RectTransform playButtonRect;

    public void Setup(Sprite levelImage, string sceneNameInGame, string sceneNameInEditor, int levelIndex)
    {
        previewImage.sprite = levelImage;
        playButton.sceneName = sceneNameInEditor;
        playButton.levelIndex = levelIndex;
        GetComponentInChildren<LoadButton>().sceneName = sceneNameInEditor;
        GetComponentInChildren<LoadButton>()._levelNameIngame = sceneNameInGame;
        title.text = sceneNameInGame;

        if (!SaveDataUtility.GetSaveFileNameIfExists(sceneNameInEditor, out string saveFilePath, out string saveFileName))
        {
            playButtonRect.anchoredPosition = new Vector2(0, playButtonRect.anchoredPosition.y);
            continueButtonObject.SetActive(false);
        }

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
