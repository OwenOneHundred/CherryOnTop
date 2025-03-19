using UnityEngine;
using System.Collections.Generic;
using EventBus;

public class OnPlaceTower : EventSO
{
    List<EventBinding<TowerPlacedEvent>> events = new();
    public override void RegisterEffect(EffectSO effectSO)
    {
        EventBinding<TowerPlacedEvent> towerPlacedBinding = new EventBinding<TowerPlacedEvent>(effectSO.OnTriggered);
        EventBus<TowerPlacedEvent>.Register(towerPlacedBinding);
    }

    public override void DeregisterAllEffects()
    {
        foreach (EventBinding<TowerPlacedEvent> towerPlacedEvent in events)
        {
            EventBus<TowerPlacedEvent>.Deregister(towerPlacedEvent);
        }
    }
}
