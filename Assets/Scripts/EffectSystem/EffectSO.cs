using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    [System.NonSerialized] public GameObject toppingObj;
    public abstract void OnTriggered(EventBus.IEvent eventObject);
}
