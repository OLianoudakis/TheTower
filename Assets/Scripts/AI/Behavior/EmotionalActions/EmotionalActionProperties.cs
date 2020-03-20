using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AI.Personality.Emotions;
using AI.Personality;

namespace AI.Behavior.EmotionalActions
{
    [Serializable]
    public struct EmotionalActionEntry
    {
        public EmotionType m_emotionType;
        public GameObject m_action;
    }

    public class EmotionalActionProperties : MonoBehaviour
    {
        [SerializeField]
        private Events.EventType m_eventType;

        private EmotionType m_triggeredEmotion;

        public Events.EventType eventType
        {
            get { return m_eventType; }
        }

        public EmotionType triggeredEmotion
        {
            get { return m_triggeredEmotion; }
            set { m_triggeredEmotion = value; }
        }
    }
}
