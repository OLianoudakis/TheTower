using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;
using Events;

namespace AI.Personality
{
    public class PersonalityManager : MonoBehaviour, ICustomEventTarget
    {
        [SerializeField]
        private PersonalityModel m_personalityModel;

        [SerializeField]
        private EventsEmotionIntensities m_eventsEmotionIntensities;

        private EmotionManager m_emotionManager = new EmotionManager();
        private MoodManager m_moodManager;
        private MotivationManager m_motivationManager = new MotivationManager();

        public void ReceiveEvent(Events.Event receivedEvent)
        {
            EventEmotionEntry entry = m_eventsEmotionIntensities.m_eventEmotionIntensity[(int)receivedEvent.m_eventType];
            //m_emotionManager.Update(entry.m_emotionType, entry.m_value);
        }

        private void Start()
        {
            m_moodManager = new MoodManager(m_personalityModel);
        }

        private void Update()
        {
            // TODO update emotions, mood and current motivation
        }
    }
}
