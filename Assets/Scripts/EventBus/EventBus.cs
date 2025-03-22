using System.Collections.Generic;
using UnityEngine;

namespace EventBus
{
    public static class EventBus<T> where T : IEvent
    {
        static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

        /// <summary>
        /// Adds listener for the event of type T.
        /// EX: Create a binding of type BuyEvent. Then, call EventBus<BuyEvent>.Register(binding).
        /// When BuyEvent event occurs, binding.OnEvent is called.
        /// </summary>
        /// <param name="binding"></param>
        public static void Register(EventBinding<T> binding) => bindings.Add(binding);
        public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

        /// <summary>
        /// Calls EventBinding.OnEvent on all registered bindings of the type.
        /// Call Raise with a reference to the static type of the event.
        /// EX: EventBus<BuyEvent>.Raise(new BuyEvent(item));
        /// </summary>
        /// <param name="event"></param>
        public static void Raise(T @event)
        {
            var snapshot = new HashSet<IEventBinding<T>>(bindings);

            foreach (var binding in snapshot)
            {
                if (bindings.Contains(binding))
                {
                    binding.OnEvent.Invoke(@event);
                    binding.OnEventNoArgs.Invoke();
                }
            }
        }

        static void Clear()
        {
            bindings.Clear();
        }
    }
}