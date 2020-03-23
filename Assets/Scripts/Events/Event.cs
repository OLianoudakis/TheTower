﻿using System.Collections;
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
        None = 0,
        PlayerSpotted = 1,
        PlayerDiscovered = 2,
        PlayerSuspicion = 3,
        EnvironmentObjectMoved = 4,
        NoiseHeard = 5
    }
}
