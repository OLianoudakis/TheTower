using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Events;

namespace AI.Personality.Emotions
{
    [Serializable]
    public struct Emotion
    {
        public EmotionType m_emotionType;
        public float m_initialIntensity;
        public float m_currentIntensity;
        public float m_initialTime;
    }

    [Serializable]
    public struct EventEmotionEntry
    {
        public Events.EventType m_eventType;
        public Emotion[] m_emotions;

        public EventEmotionEntry(Events.EventType eventType, Emotion[] emotions)
        {
            m_eventType = eventType;
            m_emotions = emotions;
        }
    }

    [CreateAssetMenu(fileName = "New Events Emotion Intensities", menuName = "Events Emotion Intensities")]
    public class EventsEmotionIntensities : ScriptableObject
    {
        [SerializeField]
        public EventEmotionEntry[] m_eventEmotionIntensity;
    }
}
