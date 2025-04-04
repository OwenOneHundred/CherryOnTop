using GameSaves;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    [System.NonSerialized]
    public GameObject toppingFirePointObj;
    public abstract void OnTriggered(EventBus.IEvent eventObject);

    public ToppingActivatedGlow GetToppingActivatedGlow()
    {
        if (toppingFirePointObj == null) { Debug.LogWarning("Failed to get toppingactivatedglow. ToppingObj is: " + toppingFirePointObj.transform.root.name); return null; }
        return toppingFirePointObj.transform.root.GetComponentInChildren<ToppingActivatedGlow>();
    }

    public string GetID()
    {
        return toppingFirePointObj.transform.root.GetComponent<ToppingObjectScript>().topping.ID.ToString();
    }

    public virtual void Save(SaveData saveData)
    {

    }

    public virtual void Load(SaveData saveData)
    {

    }
}
