﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public struct Event
    {
        public EventType m_messageType;
    }

    public enum EventType
    {
        PlayerSpotted
    }
}
