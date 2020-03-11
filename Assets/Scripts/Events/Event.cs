using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public struct Event
    {
        public EventType m_eventType;
    }

    public enum EventType
    {
        PlayerSpotted = 0
    }
}
