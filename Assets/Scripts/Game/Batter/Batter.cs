using UnityEngine;

[System.Serializable]
public class Batter : ScriptableObject
{
    [TextArea]
    public string description;
    public Color color;
    public virtual void OnGameStart()
    {

    }
}
