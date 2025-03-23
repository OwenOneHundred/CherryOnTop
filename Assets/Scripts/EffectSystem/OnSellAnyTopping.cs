using System.Collections.Generic;
using EventBus;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/OnSell")]
public class OnSellAnyTopping : EventSO
{
    List<EventBinding<SellEvent>> events = new();
    public override void RegisterEffect(EffectSO effectSO)
    {
        EventBinding<SellEvent> sellBinding = new EventBinding<SellEvent>((e) => effectSO.OnTriggered(e));
        EventBus<SellEvent>.Register(sellBinding);
    }

    public override void DeregisterAllEffects()
    {
        foreach (EventBinding<SellEvent> sellEvent in events)
        {
            EventBus<SellEvent>.Deregister(sellEvent);
        }
    }
}
