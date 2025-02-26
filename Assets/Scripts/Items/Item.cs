using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public int price = 5;
    public Sprite shopSprite;

    public List<EffectAndWhen> effectsAndWhen = new List<EffectAndWhen>();

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
        public List<EffectSO> effectSOs;
        public List<EventSO> eventSOs;
    }
}
