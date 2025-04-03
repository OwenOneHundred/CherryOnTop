using System.Collections.Generic;
using UnityEngine;
using System;
using GameSaves;

public abstract class Item : ScriptableObject
{
    public int price = 5;
    public Sprite shopSprite;
    public Guid ID;

    public List<EffectAndWhen> effectsAndWhen = new List<EffectAndWhen>();

    [TextArea] public string description;
    public ToppingTypes.Rarity rarity = ToppingTypes.Rarity.common;

    public void SetUpEffectsAndWhen()
    {
        for (int i = 0; i < effectsAndWhen.Count; i++)
        {
            for (int j = 0; j < effectsAndWhen[i].effectSOs.Count; j++)
            {   
                effectsAndWhen[i].effectSOs[j] = Instantiate(effectsAndWhen[i].effectSOs[j]);
            }
            for (int p = 0; p < effectsAndWhen[i].eventSOs.Count; p++)
            {   
                effectsAndWhen[i].eventSOs[p] = Instantiate(effectsAndWhen[i].eventSOs[p]);
            }
        }
    }

    /// <summary>
    /// Should be called when the item is purchased. Registers the events in the effects list.
    /// </summary>
    public void RegisterEffects()
    {
        foreach (EffectAndWhen effectAndWhen in effectsAndWhen)
        {
            foreach (EffectSO effectSO in effectAndWhen.effectSOs)
            {
                foreach (BaseEventSO eventSO in effectAndWhen.eventSOs)
                {
                    eventSO.RegisterEffect(effectSO);
                }
            }
        }
    }

    /// <summary>
    /// Should be called when the item is sold, destroyed, or otherwise removed from the inventory or the map.
    /// </summary>
    public void DeregisterEffects()
    {
        foreach (EffectAndWhen effectAndWhen in effectsAndWhen)
        {
            foreach (BaseEventSO eventSO in effectAndWhen.eventSOs)
            {
                eventSO.DeregisterAllEffects();
            }
        }
    }

    public void LoadToppingData(SaveData saveData)
    {
        foreach (EffectAndWhen effectAndWhen in effectsAndWhen)
        {
            foreach (EffectSO effectSO in effectAndWhen.effectSOs)
            {
                effectSO.Load(saveData);
            }
        }
    }

    public void SaveToppingData(SaveData saveData)
    {
        foreach (EffectAndWhen effectAndWhen in effectsAndWhen)
        {
            foreach (EffectSO effectSO in effectAndWhen.effectSOs)
            {
                effectSO.Save(saveData);
            }
        }
    }

    [System.Serializable]
    public class EffectAndWhen
    {
        /// <summary>
        /// What happens when any of the events occur
        /// </summary>
        public List<EffectSO> effectSOs;

        /// <summary>
        /// What events trigger the above effects
        /// </summary>
        public List<BaseEventSO> eventSOs;
    }
}
