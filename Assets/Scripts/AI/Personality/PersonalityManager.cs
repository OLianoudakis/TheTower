using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;
using AI.Behavior;
using Events;

namespace AI.Personality
{
    public class PersonalityManager : MonoBehaviour, ICustomEventTarget
    {
        [SerializeField]
        private PersonalityModel m_personalityModel;

        [SerializeField]
        private EventsEmotionIntensities m_eventsEmotionIntensities;

        [SerializeField]
        private float m_personalityUpdateCooldown = 0.3f;

        private float m_currentPersonalityCooldown = 0.0f;

        private EmotionManager m_emotionManager = new EmotionManager();
        private MoodManager m_moodManager;
        private MotivationManager m_motivationManager;
        private BehaviorManager m_behaviorManager;

        public void ReceiveEvent(Events.Event receivedEvent)
        {
            EventEmotionEntry entry = m_eventsEmotionIntensities.m_eventEmotionIntensity[(int)receivedEvent.m_eventType];
            Emotion[] modifiedEmotions = m_emotionManager.AddEmotions(entry.m_emotions, m_moodManager.mood, m_personalityModel);
            entry.m_emotions = modifiedEmotions;
            m_behaviorManager.AddGeneratedEmotion(entry);
        }

        public float[] GetCurrentDesires()
        {
            return m_motivationManager.GetCurrentDesires(m_emotionManager.activeEmotions, m_emotionManager.emotionIntensityLowerBound);
        }

        private void Start()
        {
            m_moodManager = new MoodManager(m_personalityModel);
            m_motivationManager = new MotivationManager(m_personalityModel);
            m_behaviorManager = GetComponent(typeof(BehaviorManager)) as BehaviorManager;
        }

        private void Update()
        {
            m_currentPersonalityCooldown += Time.deltaTime;
            if (m_currentPersonalityCooldown >= m_personalityUpdateCooldown)
            {
                m_currentPersonalityCooldown = 0.0f;
                m_motivationManager.UpdateCurrentMotivations(m_behaviorManager.currentMotivationGain, Time.deltaTime);
                m_emotionManager.DecayEmotionIntensity();
                m_moodManager.UpdateMood(m_emotionManager.activeEmotions);
            }
        }
    }
}
