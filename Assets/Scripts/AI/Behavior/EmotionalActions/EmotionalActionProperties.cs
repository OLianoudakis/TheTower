using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AI.Personality.Emotions;

namespace AI.Behavior.EmotionalActions
{
    [Serializable]
    public struct EmotionalActionEntry
    {
        public Emotion[] m_emotions;
        public GameObject m_action;
    }

    public class EmotionalActionProperties : MonoBehaviour
    {
        [SerializeField]
        private Events.EventType m_eventType;

        public Events.EventType eventType
        {
            get { return m_eventType; }
        }
    }
}
