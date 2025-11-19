using UnityEngine;

public class EventBusManager : MonoBehaviour
{
    public EventBus EventBus { get; private set; }

    public void Initialize()
    {
        EventBus = new EventBus();
    }
}