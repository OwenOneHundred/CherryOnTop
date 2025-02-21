using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelName", menuName = "Scriptable Objects/LevelPreview")]
public class LevelPreview : ScriptableObject
{
    public GameObject emptyLevelPrefab;
    public Sprite levelImage;
    public string sceneNameInEditor;

    void Start()
    {
        GameObject panel = emptyLevelPrefab.transform.Find("Panel").gameObject;
        panel.GetComponent<Image>().sprite = levelImage;
        GameObject sceneChangeButton = emptyLevelPrefab.transform.Find("Scene Change Button").gameObject;
        sceneChangeButton.GetComponent<PlayButton>().sceneName = sceneNameInEditor;
    }

}
