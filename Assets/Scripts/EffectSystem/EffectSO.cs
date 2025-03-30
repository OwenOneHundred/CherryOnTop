using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    [System.NonSerialized] public GameObject toppingObj;
    public abstract void OnTriggered(EventBus.IEvent eventObject);

    public ToppingActivatedGlow GetToppingActivatedGlow()
    {
        if (toppingObj == null) { Debug.LogWarning("Failed to get toppingactivatedglow. ToppingObj is: " + toppingObj);}
        return toppingObj.GetComponentInChildren<ToppingActivatedGlow>();
    }
}
