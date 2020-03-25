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
        PlayerSpotted = 0,
        PlayerDiscovered = 1,
        PlayerSuspicion = 2,
        EnvironmentObjectMoved = 3,
        NoiseHeard = 4,
        PlayerLost = 5,
        NoiseHeardBySomebodyElse = 6
    }
}
