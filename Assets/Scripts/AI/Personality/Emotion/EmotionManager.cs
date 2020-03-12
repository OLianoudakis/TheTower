using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;


namespace AI.Personality.Emotions
{
    public class EmotionManager : ICustomEventTarget
    {
        [SerializeField]
        private EventsEmotionIntensities m_eventsEmotionIntensities;

        private List<Emotion> m_activeEmotions = new List<Emotion>();
        private const float m_decayingConstant = 2.0f;

        public List<Emotion> activeEmotions
        {
            get { return m_activeEmotions; }
        }

        public void ReceiveEvent(Events.Event receivedEvent)
        {
            EventEmotionEntry entry = m_eventsEmotionIntensities.m_eventEmotionIntensity[(int)receivedEvent.m_eventType];
            AddEmotion(entry.m_emotion.m_emotionType, entry.m_emotion.m_initialIntensity);
        }

        public void DecreaseEmotionIntensity()
        {
            if (m_activeEmotions.Count <= 0)
            {
                return;
            }

            for (int i = m_activeEmotions.Count; i > -1; i--)
            {
                Emotion currentEmotion = m_activeEmotions[i];
                currentEmotion.m_currentIntensity = m_activeEmotions[i].m_initialIntensity * Mathf.Pow(Mathf.Epsilon, -m_decayingConstant * (Time.time - m_activeEmotions[i].m_initialTime));
                m_activeEmotions[i] = currentEmotion;

                if (m_activeEmotions[i].m_currentIntensity <= 0.0f)
                {
                    m_activeEmotions.RemoveAt(i);
                }
            }
        }

        private void AddEmotion(EmotionType emotionType, float intensity)
        {
            Emotion emotion = new Emotion();
            emotion.m_emotionType = emotionType;
            emotion.m_initialIntensity = intensity;
            emotion.m_currentIntensity = intensity;
            emotion.m_initialTime = Time.time;
            m_activeEmotions.Add(emotion);
        }
    }
}
