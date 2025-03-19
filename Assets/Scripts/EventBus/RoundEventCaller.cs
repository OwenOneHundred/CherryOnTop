using EventBus;
using UnityEngine;

// This class is an example of using the events
// This class is NOT used ingame
public class RoundEventCaller : MonoBehaviour
{
    // have an event to register for the type of RoundStartEvent
    EventBinding<RoundStartEvent> roundEvent;

    private void OnEnable()
    {
        // Create the event and register it to the event bus
        // To make the event have a different function, just use another function that
        // either accepts no parameter or accepts the RoundStartEvent handler.
        roundEvent = new EventBinding<RoundStartEvent>(HandleRoundStartEvent);
        EventBus<RoundStartEvent>.Register(roundEvent);
    }

    private void OnDisable()
    {
        // Deregister the event from the bus
        EventBus<RoundStartEvent>.Deregister(roundEvent);
    }

    // Update method to, based on input, call the event (just to make sure it works)
    bool keypressed = false;
    uint roundNumber = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !keypressed)
        {
            keypressed = true;
            EventBus<RoundStartEvent>.Raise(new RoundStartEvent{ roundNumber = roundNumber });
            roundNumber++;
        } else if (!Input.GetKeyDown(KeyCode.Space))
        {
            keypressed = false;
        }
    }

    // This function is used whenever the event is called.
    public void HandleRoundStartEvent(RoundStartEvent roundStartEvent)
    {
        Debug.Log("Calling round start event: " + roundStartEvent.roundNumber);
    }
}
