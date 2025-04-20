using System.Collections.Generic;
using UnityEngine;
using System;
using GameSaves;
using static ToppingActivatedGlow;

public abstract class Item : ScriptableObject
{
    public int price = 5;
    public Sprite shopSprite;
    public Guid ID;

    public List<EffectAndWhen> effectsAndWhen = new List<EffectAndWhen>();

    [TextArea] public string description;
    public ToppingTypes.Rarity rarity = ToppingTypes.Rarity.Common;

    public void SetUpEffectsAndWhen(GameObject obj = null)
    {
        List<EffectAndWhen> effectsAndWhens = new List<EffectAndWhen>(effectsAndWhen.Count);
        for (int i = 0; i < effectsAndWhen.Count; i++)
        {
            EffectAndWhen effectAndWhen = new EffectAndWhen() { 
                effectSOs = new List<EffectSO>(effectsAndWhen[i].effectSOs.Count), 
                eventSOs = new List<BaseEventSO>(effectsAndWhen[i].eventSOs.Count) };
            for (int j = 0; j < effectsAndWhen[i].effectSOs.Count; j++)
            {
                EffectSO effect = Instantiate(effectsAndWhen[i].effectSOs[j]);
                if (obj != null) effect.toppingObj = obj;
                effectAndWhen.effectSOs.Add(effect);
                //effectsAndWhen[i].effectSOs[j] = Instantiate(effectsAndWhen[i].effectSOs[j]);
            }
            for (int p = 0; p < effectsAndWhen[i].eventSOs.Count; p++)
            {
                effectAndWhen.eventSOs.Add(Instantiate(effectsAndWhen[i].eventSOs[p]));
                //effectsAndWhen[i].eventSOs[p] = Instantiate(effectsAndWhen[i].eventSOs[p]);
            }
            effectsAndWhens.Add(effectAndWhen);
        }
        effectsAndWhen = effectsAndWhens;
    }

    /// <summary>
    /// Should be called when the item is purchased. Registers the events in the effects list.
    /// </summary>
    public void RegisterEffects(GameObject toppingObj = null)
    {
        foreach (EffectAndWhen effectAndWhen in effectsAndWhen)
        {
            foreach (EffectSO effectSO in effectAndWhen.effectSOs)
            {
                foreach (BaseEventSO eventSO in effectAndWhen.eventSOs)
                {
                    if (toppingObj != null)
                    {
                        effectSO.toppingObj = toppingObj;
                    }
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
                Debug.Log(ID.ToString() + ": Dereigstered all effects under event " + eventSO.name);
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
