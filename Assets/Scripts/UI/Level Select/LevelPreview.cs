using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelName", menuName = "Scriptable Objects/LevelPreview")]
public class LevelPreview : ScriptableObject
{
    public GameObject emptyLevelPrefab;
    public GameObject levelPrefab;
    public Sprite levelImage;
    public string sceneNameInEditor;
}
