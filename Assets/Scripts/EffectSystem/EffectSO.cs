using GameSaves;
using UnityEngine;

public abstract class EffectSO : ScriptableObject
{
    [System.NonSerialized]
    public GameObject toppingObj;
    public abstract void OnTriggered(EventBus.IEvent eventObject);
    [SerializeField] AudioFile triggeredSound;

    public ToppingActivatedGlow GetToppingActivatedGlow()
    {
        if (toppingObj == null) { Debug.LogWarning("Failed to get toppingactivatedglow. ToppingObj is null."); return null; }
        return toppingObj.transform.root.GetComponentInChildren<ToppingActivatedGlow>();
    }

    public string GetID()
    {
        Debug.Log("Get ID on " + this.name + " on " + toppingObj);
        return toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping.ID.ToString();
    }

    public void PlayTriggeredSound()
    {
        if (triggeredSound.clip != null)
        {
            SoundEffectManager.sfxmanager.PlayOneShot(triggeredSound);
        }
    }

    public virtual void OnRegistered()
    {

    }

    public virtual void OnDeregistered()
    {

    }

    public Topping GetTopping()
    {
        if (toppingObj == null)
        {
            Debug.LogError("No toppingObj found for effect " + this.name);
            return null;
        }
        else return toppingObj.transform.root.GetComponent<ToppingObjectScript>().topping;
    }

    public virtual void Save(SaveData saveData)
    {

    }

    public virtual void Load(SaveData saveData)
    {

    }
}
