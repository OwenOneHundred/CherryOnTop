using UnityEngine;
using System.Collections.Generic;
using EventBus;

[CreateAssetMenu(menuName = "Event/OnTowerPlaced")]
public class OnPlaceTower : EventSO
{
    List<EventBinding<TowerPlacedEvent>> events = new();
    public override void RegisterEffect(EffectSO effectSO)
    {
        EventBinding<TowerPlacedEvent> towerPlacedBinding = new EventBinding<TowerPlacedEvent>((e) => effectSO.OnTriggered(e));
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
