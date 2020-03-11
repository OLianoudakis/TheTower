using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Events;

namespace AI.Personality.Emotion
{
    [Serializable]
    public struct EventEmotionEntry
    {
        public Events.EventType m_eventType;
        public EmotionType m_emotionType;
        public float m_value;

        public EventEmotionEntry(Events.EventType eventType, EmotionType emotionType, float value)
        {
            m_eventType = eventType;
            m_emotionType = emotionType;
            m_value = value;
        }
    }

    [CreateAssetMenu(fileName = "New Events Emotion Intensities", menuName = "Events Emotion Intensities")]
    public class EventsEmotionIntensities : ScriptableObject
    {
        [SerializeField]
        public EventEmotionEntry[] m_eventEmotionIntensity;
    }
}
