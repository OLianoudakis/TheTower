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
        private bool m_usePersonalityModel = true;

        [SerializeField]
        private float m_emotionIntensityTreshold = 0.0f;

        [SerializeField]
        private float m_motivationNegativeInfluenceThreshold = 0.1f;

        [SerializeField]
        private float m_behaviorUpdateCooldown = 0.3f;

        private float m_currentBehaviorCooldown = 0.0f;
        private PersonalityManager m_personalityManager;
        private List<EventEmotionEntry> m_generatedEmotionalEvents = new List<EventEmotionEntry>();
        private List<MotivationActionProperties> m_motivationActionProperties = new List<MotivationActionProperties>();
        private EmotionalActionProperties m_emotionalProperties;
        private GameObject m_currentlyActivatedMotivationAction = null;
        private float[] m_currentMotivationGain = null;
        private bool m_behaviorInterrupted = false;

        public float[] currentMotivationGain
        {
            get { return m_currentMotivationGain; }
        }

        public void AddGeneratedEmotion(EventEmotionEntry emotionEvent)
        {
            m_generatedEmotionalEvents.Add(emotionEvent);
        }

        public void ContinueBehavior()
        {
            if (m_currentlyActivatedMotivationAction)
            {
                m_currentlyActivatedMotivationAction.SetActive(true);
                m_behaviorInterrupted = false;
            }
        }

        public void InterruptBehavior()
        {
            if (m_currentlyActivatedMotivationAction)
            {
                m_currentlyActivatedMotivationAction.SetActive(false);
                m_behaviorInterrupted = true;
            }
        }

        private bool ActivateEmotionalAction()
        {
            if (m_generatedEmotionalEvents.Count == 0)
            {
                return false;
            }
            // pick the emotion with the strongest intensity
            EventEmotionEntry emotionalEventToTrigger;
            emotionalEventToTrigger.m_eventType = Events.EventType.PlayerSpotted;
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

            m_emotionalProperties.eventType = emotionalEventToTrigger.m_eventType;
            m_emotionalProperties.triggeredEmotion = strongestEmotion;
            m_emotionalProperties.ActivateAction();

            m_generatedEmotionalEvents.Clear();

            return false;
        }

        private void ActivateMotivationAction()
        {
            MotivationActionProperties chosenAction = ChooseMotivationAction();
            
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

        private void Awake()
        {
            m_personalityManager = GetComponent(typeof(PersonalityManager)) as PersonalityManager;
            for (int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).gameObject.activeInHierarchy)
                {
                    GameObject action = transform.GetChild(i).gameObject;
                    MotivationActionProperties motivationAction = action.GetComponent(typeof(MotivationActionProperties)) as MotivationActionProperties;
                    if (motivationAction)
                    {
                        m_motivationActionProperties.Add(motivationAction);
                    }
                }
            }
            m_emotionalProperties = GetComponentInChildren(typeof(EmotionalActionProperties)) as EmotionalActionProperties;
        }

        private MotivationActionProperties ChooseMotivationAction()
        {
            MotivationActionProperties chosenAction = null;
            float[] currentDesires = null;
            float distance = 100.0f;
            int priority = -1;
            int mostSignificantMotivation = 0;
            float currentMostSignificantMotivationValue = 0.0f;
            if (m_usePersonalityModel)
            {
                currentDesires = m_personalityManager.GetCurrentDesires(ref mostSignificantMotivation);
            }
            foreach (MotivationActionProperties motivationActionProperties in m_motivationActionProperties)
            {
                if (m_usePersonalityModel)
                {
                    float newDistance = 0.0f;
                    for (int i = 0; i < motivationActionProperties.motivationGain.m_motivationDesiresGain.Length; i++)
                    {
                        float motivationGain = motivationActionProperties.motivationGain.m_motivationDesiresGain[i].m_value;
                        if (currentDesires[i] < m_motivationNegativeInfluenceThreshold)
                        {
                            if (currentDesires[i] == 0.0f)
                            {
                                motivationGain = 0.0f;
                            }
                            else
                            {
                                motivationGain /= 2.0f;
                            }
                        }
                        newDistance += currentDesires[i] - motivationGain;
                        Debug.Log(motivationActionProperties.name + " " + motivationActionProperties.motivationGain.m_motivationDesiresGain[i].m_motivationDesire.ToString() + " " 
                            + currentDesires[i].ToString() + " " + newDistance.ToString());
                    }

                    if ((
                        (newDistance < distance) 
                        || 
                        ((newDistance == distance) 
                        && (motivationActionProperties.motivationGain.m_motivationDesiresGain[mostSignificantMotivation].m_value > currentMostSignificantMotivationValue))
                        ) 
                        && motivationActionProperties.CanBeTriggered())
                    {
                        currentMostSignificantMotivationValue = motivationActionProperties.motivationGain.m_motivationDesiresGain[mostSignificantMotivation].m_value;
                        chosenAction = motivationActionProperties;
                        distance = newDistance;
                    }
                    continue;
                }

                if ((motivationActionProperties.priority > 0.0f) && ((priority < 0.0f) || (motivationActionProperties.priority < priority))
                    && motivationActionProperties.CanBeTriggered())
                {
                    chosenAction = motivationActionProperties;
                    priority = motivationActionProperties.priority;
                }
            }
            return chosenAction;
        }

        private void Update()
        {
            if (!m_behaviorInterrupted)
            {
                m_currentBehaviorCooldown += Time.deltaTime;
                if (m_usePersonalityModel && ActivateEmotionalAction())
                {
                    return;
                }
                if (m_currentBehaviorCooldown >= m_behaviorUpdateCooldown)
                {
                    m_currentBehaviorCooldown = 0.0f;
                    ActivateMotivationAction();
                }
            }
        }
    }
}
