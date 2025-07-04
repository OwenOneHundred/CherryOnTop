using UnityEngine;
using EventBus;
using System.Collections.Generic;

public abstract class EventSO <T> : BaseEventSO where T : IEvent
{
    [SerializeField] List<EventBinding<T>> events = new List<EventBinding<T>>();

    public override void RegisterEffect(EffectSO effectSO)
    {
        EventBinding<T> towerPlacedBinding = new EventBinding<T>((e) => effectSO.OnTriggered(e));
        EventBus<T>.Register(towerPlacedBinding);
        events.Add(towerPlacedBinding);
    }

    public override void DeregisterAllEffects()
    {
        foreach (EventBinding<T> eventObject in events)
        {
            EventBus<T>.Deregister(eventObject);
        }
        events.Clear();
    }
}

public abstract class BaseEventSO : ScriptableObject
{
    public abstract void RegisterEffect(EffectSO effectSO);
    public abstract void DeregisterAllEffects();
}