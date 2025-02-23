using UnityEngine;
using EventBus;
using System.Collections.Generic;

public abstract class EventSO : ScriptableObject
{
    public abstract void RegisterEffect(EffectSO effectSO);
    public abstract void DeregisterAllEffects();
}
