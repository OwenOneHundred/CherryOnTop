using UnityEngine;

public abstract class RewardItem : ScriptableObject
{
    public float weight = 1;
    public abstract void OnClaim(float value);
}
