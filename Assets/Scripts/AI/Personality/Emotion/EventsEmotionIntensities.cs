using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AI.Personality.Emotion
{
    [Serializable]
    public struct EventEmotionEntry
    {
        public EventType m_eventType;
        public EmotionType m_emotionType;
        public float m_value;

        public EventEmotionEntry(EventType eventType, EmotionType emotionType, float value)
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
        private EventEmotionEntry[] m_personalityTraitsValues;
    }
}
