using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelName", menuName = "Scriptable Objects/LevelPreview")]
[System.Serializable]
public class LevelPreview : ScriptableObject
{   
    public GameObject levelPrefab;
    public Sprite levelImage;
    public string sceneNameInEditor;
    public string sceneNameIngame;
}
