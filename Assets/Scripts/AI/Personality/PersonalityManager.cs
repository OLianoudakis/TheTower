using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;
using AI.Behavior;
using Events;

namespace AI.Personality
{
    public class PersonalityManager : MonoBehaviour , ICustomEventTarget
    {
        [SerializeField]
        private PersonalityModel m_personalityModel;

        [SerializeField]
        private EventsEmotionIntensities m_eventsEmotionIntensities;

        private EmotionManager m_emotionManager = new EmotionManager();
        private MoodManager m_moodManager;
        private MotivationManager m_motivationManager;
        private BehaviorManager m_behaviorManager;

        public void ReceiveEvent(Events.Event receivedEvent)
        {
            EventEmotionEntry entry = m_eventsEmotionIntensities.m_eventEmotionIntensity[(int)receivedEvent.m_eventType];
            m_emotionManager.AddEmotion(entry.m_emotion, m_moodManager.mood, m_personalityModel);
        }

        public float[] GetCurrentDesires()
        {
            return m_motivationManager.GetCurrentDesires(m_emotionManager.activeEmotions, m_emotionManager.emotionIntensityLowerBound);
        }

        private void Start()
        {
            m_moodManager = new MoodManager(m_personalityModel);
            m_motivationManager = new MotivationManager(m_personalityModel);
            m_behaviorManager = transform.parent.GetComponentInChildren(typeof(BehaviorManager)) as BehaviorManager;
        }

        private void Update()
        {
            m_motivationManager.UpdateCurrentMotivations(m_behaviorManager.finishedMotivations);
            m_emotionManager.DecayEmotionIntensity();
            m_moodManager.UpdateMood(m_emotionManager.activeEmotions);
        }
    }
}
