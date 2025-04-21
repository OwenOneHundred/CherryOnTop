using UnityEngine;
using UnityEngine.UI;

public class LevelPreviewManager : MonoBehaviour
{
    [SerializeField] Image previewImage;
    [SerializeField] PlayButton playButton;
    [SerializeField] TMPro.TextMeshProUGUI title;
    public void Setup(Sprite levelImage, string sceneNameInGame, string sceneNameInEditor)
    {
        previewImage.sprite = levelImage;
        playButton.sceneName = sceneNameInEditor;
        GetComponentInChildren<LoadButton>().sceneName = sceneNameInEditor;
        GetComponentInChildren<LoadButton>()._levelNameIngame = sceneNameInGame;
        title.text = sceneNameInGame;
    }
}
