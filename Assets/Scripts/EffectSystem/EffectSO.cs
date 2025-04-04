using GameSaves;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    [System.NonSerialized]
    public GameObject toppingObj;
    public abstract void OnTriggered(EventBus.IEvent eventObject);

    public ToppingActivatedGlow GetToppingActivatedGlow()
    {
        if (toppingObj == null) { Debug.LogWarning("Failed to get toppingactivatedglow. ToppingObj is: " + toppingObj.transform.root.name); return null; }
        return toppingObj.transform.root.GetComponentInChildren<ToppingActivatedGlow>();
    }

    public string GetID()
    {
        return toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping.ID.ToString();
    }

    public virtual void Save(SaveData saveData)
    {

    }

    public virtual void Load(SaveData saveData)
    {

    }
}
