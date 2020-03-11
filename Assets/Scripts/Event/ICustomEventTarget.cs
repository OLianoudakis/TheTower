using UnityEngine.EventSystems;

namespace Events
{
    public interface ICustomEventTarget : IEventSystemHandler
    {
        void ReceiveEvent(Event receivedEvent);
    }
}
