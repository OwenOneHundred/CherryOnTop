using UnityEngine;

[System.Serializable]
public class Batter : ScriptableObject
{
    [TextArea]
    public string description;
    public Color color;
    public string visibleName;
    public virtual void OnGameStart()
    {

    }
}
