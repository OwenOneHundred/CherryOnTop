using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int price = 5;
    public Sprite shopSprite;

    public List<EffectAndWhen> effectsAndWhen = new List<EffectAndWhen>();

    [TextArea] public string description;

    /// <summary>
    /// Should be called when the item is purchased. Registers the events in the effects list.
    /// </summary>
    public void RegisterEffects()
    {
        foreach (EffectAndWhen effectAndWhen in effectsAndWhen)
        {
            foreach (EffectSO effectSO in effectAndWhen.effectSOs)
            {
                foreach (EventSO eventSO in effectAndWhen.eventSOs)
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
            foreach (EventSO eventSO in effectAndWhen.eventSOs)
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
        public List<EventSO> eventSOs;
    }
}
