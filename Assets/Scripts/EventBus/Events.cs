using UnityEngine;

namespace EventBus
{
    public interface IEvent { }

    public struct TestEvent : IEvent { }

    public struct RoundStartEvent : IEvent
    {
        public uint roundNumber;
    }

    public struct TowerPlacedEvent : IEvent
    {
        // add variable corresponding to a tower
    }

    public struct CherryHitEvent : IEvent
    {
        public CherryHitbox cherry; // replace with better reference to the cherry
    }

    // Create other event structs here to indicate kinds of different events
    // For each new IEvent struct declared, a new EventBus<?> will be created, where
    // ? is the type of IEvent struct declared.
}