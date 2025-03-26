using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int price = 5;
    public Sprite shopSprite;

    public List<EffectAndWhen> effectsAndWhen = new List<EffectAndWhen>();

    [TextArea] public string description;

    public void Initialize()
    {
        SetUpEffectsAndWhen();

        RegisterEffects();
    }

    private void SetUpEffectsAndWhen()
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
    private void RegisterEffects()
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
