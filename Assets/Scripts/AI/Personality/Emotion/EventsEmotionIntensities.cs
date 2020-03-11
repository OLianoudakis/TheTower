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
        public float m_value;
    }

    [Serializable]
    public struct EventEmotionEntry
    {
        public Events.EventType m_eventType;
        public Emotion m_emotion;

        public EventEmotionEntry(Events.EventType eventType, Emotion emotion)
        {
            m_eventType = eventType;
            m_emotion = emotion;
        }
    }

    [CreateAssetMenu(fileName = "New Events Emotion Intensities", menuName = "Events Emotion Intensities")]
    public class EventsEmotionIntensities : ScriptableObject
    {
        [SerializeField]
        public EventEmotionEntry[] m_eventEmotionIntensity;
    }
}
