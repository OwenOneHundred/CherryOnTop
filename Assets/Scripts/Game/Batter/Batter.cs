using UnityEngine;

[System.Serializable]
public class Batter : ScriptableObject
{
    [TextArea]
    public string description;
    public virtual void OnGameStart()
    {

    }
}
