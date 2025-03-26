using UnityEngine;

namespace EventBus
{
    public interface IEvent { }

    public struct TestEvent : IEvent { }

    public struct RoundStartEvent : IEvent
    {
        public RoundStartEvent(uint roundNumber)
        {
            this.roundNumber = roundNumber;
        }
        public uint roundNumber;
    }

    public struct BuyEvent : IEvent
    {
        public BuyEvent(Item item)
        {
            this.item = item;
        }
        public Item item;
    }

    public struct SellEvent : IEvent
    {
        public SellEvent(Item item, GameObject toppingObj)
        {
            this.item = item;
            this.toppingObj = toppingObj;
        }
        public Item item;
        public GameObject toppingObj; // null if ingredient
    }

    public struct TowerPlacedEvent : IEvent
    {
        public TowerPlacedEvent(Topping topping, GameObject newToppingObj)
        {
            this.topping = topping;
            this.newToppingObj = newToppingObj;
        }
        public Topping topping;
        public GameObject newToppingObj;
    }

    public struct CherryHitEvent : IEvent
    {
        public CherryHitbox cherry; // replace with better reference to the cherry
    }

    // Create other event structs here to indicate kinds of different events
    // For each new IEvent struct declared, a new EventBus<?> will be created, where
    // ? is the type of IEvent struct declared.

}