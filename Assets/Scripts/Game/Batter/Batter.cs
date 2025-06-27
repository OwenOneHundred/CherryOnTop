using UnityEngine;

[System.Serializable]
public class Batter : ScriptableObject
{
    [TextArea]
    public string description;
    public Color color;
    public string visibleName;
    public int index = 0;
    public Difficulty associatedDifficulty;
    public virtual void OnGameStart()
    {

    }
}
