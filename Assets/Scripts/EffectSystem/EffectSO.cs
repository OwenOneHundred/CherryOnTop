using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    public GameObject toppingObj;
    public abstract void OnTriggered(EventBus.IEvent eventObject);
}
