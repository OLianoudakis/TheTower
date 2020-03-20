using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;
using Events;
using AI.Behavior.MotivationActions;
using AI.Behavior.EmotionalActions;
using AI.Personality;

namespace AI.Behavior
{
    public class BehaviorManager : MonoBehaviour
    {
        [SerializeField]
        private float m_emotionIntensityTreshold = 0.6f;

        [SerializeField]
        private float m_behaviorUpdateCooldown = 0.5f;

        private float m_currentBehaviorCooldown = 0.0f;
        private PersonalityManager m_personalityManager;
        private List<EventEmotionEntry> m_generatedEmotionalEvents = new List<EventEmotionEntry>();
        private List<MotivationActionProperties> m_motivationActionProperties = new List<MotivationActionProperties>();
        private List<EmotionalActionProperties> m_emotionalProperties = new List<EmotionalActionProperties>();
        private GameObject m_currentlyActivatedMotivationAction = null;
        private float[] m_currentMotivationGain = null;

        public float[] currentMotivationGain
        {
            get { return m_currentMotivationGain; }
        }

        public void AddGeneratedEmotion(EventEmotionEntry emotionEvent)
        {
            m_generatedEmotionalEvents.Add(emotionEvent);
        }

        private bool ActivateEmotionalAction()
        {
            // pick the emotion with the strongest intensity
            EventEmotionEntry emotionalEventToTrigger;
            emotionalEventToTrigger.m_eventType = Events.EventType.None;
            float strongestIntensity = 0.0f;
            EmotionType strongestEmotion = EmotionType.Admiration;
            foreach (EventEmotionEntry emotionalEvent in m_generatedEmotionalEvents)
            {
                foreach (Emotion emotion in emotionalEvent.m_emotions)
                {
                    if (emotion.m_initialIntensity > strongestIntensity)
                    {
                        strongestIntensity = emotion.m_initialIntensity;
                        emotionalEventToTrigger = emotionalEvent;
                        strongestEmotion = emotion.m_emotionType;
                    }
                }
            }

            // dont trigger any emotional action if threshold not surpassed
            if (strongestIntensity < m_emotionIntensityTreshold)
            {
                m_generatedEmotionalEvents.Clear();
                return false;
            }

            foreach (EmotionalActionProperties emotionalProperty in m_emotionalProperties)
            {
                if ((emotionalProperty.eventType == emotionalEventToTrigger.m_eventType))
                {
                    emotionalProperty.triggeredEmotion = strongestEmotion;
                    emotionalProperty.gameObject.SetActive(true);
                    break;
                }
            }

            m_generatedEmotionalEvents.Clear();

            return false;
        }

        private void ActivateMotivationAction()
        {
            float[] currentDesires = m_personalityManager.GetCurrentDesires();
            float distance = 100.0f;
            MotivationActionProperties chosenAction = null;
            foreach (MotivationActionProperties motivationActionProperties in m_motivationActionProperties)
            {
                float newDistance = 0.0f;   
                for (int i = 0; i < motivationActionProperties.motivationGain.m_motivationDesiresGain.Length; i++)
                {
                    newDistance += Mathf.Abs(currentDesires[i] - motivationActionProperties.motivationGain.m_motivationDesiresGain[i].m_value);
                }
                // TODO here also check if the rule holds (ie - player spotted etc)
                if ((newDistance < distance) && motivationActionProperties.CanBeTriggered())
                {
                    chosenAction = motivationActionProperties;
                }
            }
            if (chosenAction && !chosenAction.gameObject.activeInHierarchy)
            {
                if (m_currentlyActivatedMotivationAction)
                {
                    m_currentlyActivatedMotivationAction.SetActive(false);
                }
                chosenAction.gameObject.SetActive(true);
                m_currentlyActivatedMotivationAction = chosenAction.gameObject;
                m_currentMotivationGain = chosenAction.motivationGain.gainAsArray;
            }
        }

        private void Start()
        {
            m_personalityManager = GetComponent(typeof(PersonalityManager)) as PersonalityManager;
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject action = transform.GetChild(i).gameObject;
                MotivationActionProperties motivationAction = action.GetComponent(typeof(MotivationActionProperties)) as MotivationActionProperties;
                if (motivationAction)
                {
                    m_motivationActionProperties.Add(motivationAction);
                }
                else
                {
                    m_emotionalProperties.Add(action.GetComponent(typeof(EmotionalActionProperties)) as EmotionalActionProperties);
                }
                action.SetActive(false);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            m_currentBehaviorCooldown += Time.deltaTime;
            if (!ActivateEmotionalAction())
            {
                if (m_currentBehaviorCooldown >= m_behaviorUpdateCooldown)
                {
                    m_currentBehaviorCooldown = 0.0f;
                    ActivateMotivationAction();
                }
            }
        }
    }
}
