﻿using System.Collections;
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
        private float m_behaviorUpdateCooldown = 0.5f;

        private float m_currentBehaviorCooldown = 0.0f;
        private float m_cooldown = 0.0f;
        private PersonalityManager m_personalityManager;
        private List<EventEmotionEntry> m_generatedEmotionalEvents = new List<EventEmotionEntry>();
        private List<MotivationActionProperties> m_motivationActionProperties = new List<MotivationActionProperties>();
        private EmotionalActionProperties m_emotionalProperties;
        private GameObject m_currentlyActivatedMotivationAction = null;
        private float[] m_currentMotivationGain = null;
        private bool m_behaviorInterrupted = false;

        public float[] currentMotivationGain
        {
            get
            {
                if (m_currentlyActivatedMotivationAction)
                {
                    return m_currentMotivationGain;
                }
                return null;
            }
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
            Emotion strongestEmotion;
            strongestEmotion.m_emotionType = EmotionType.Admiration;
            strongestEmotion.m_emotionalTimeResponse = m_behaviorUpdateCooldown;
            foreach (EventEmotionEntry emotionalEvent in m_generatedEmotionalEvents)
            {
                foreach (Emotion emotion in emotionalEvent.m_emotions)
                {
                    if (emotion.m_initialIntensity > strongestIntensity)
                    {
                        strongestIntensity = emotion.m_initialIntensity;
                        emotionalEventToTrigger = emotionalEvent;
                        strongestEmotion = emotion;
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
            m_emotionalProperties.triggeredEmotion = strongestEmotion.m_emotionType;
            m_emotionalProperties.ActivateAction();

            // turn off current motivation action for time specified in emotion
            if (m_currentlyActivatedMotivationAction)
            {
                m_currentlyActivatedMotivationAction.SetActive(false);
            }
            if (strongestEmotion.m_emotionalTimeResponse > 0.0f)
            {
                m_cooldown = 0.0f;
                m_currentBehaviorCooldown = strongestEmotion.m_emotionalTimeResponse;
            }

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
                        // TODO probably remove, not needed 
                        //MotivationGain newMotivationGain = ScriptableObject.CreateInstance(typeof(MotivationGain)) as MotivationGain;
                        //newMotivationGain.m_motivationDesiresGain = motivationAction.motivationGain.m_motivationDesiresGain;
                        //newMotivationGain.gainAsArray = motivationAction.motivationGain.gainAsArray;
                        //motivationAction.motivationGain = newMotivationGain;

                        m_motivationActionProperties.Add(motivationAction);
                    }
                }
            }
            m_emotionalProperties = GetComponentInChildren(typeof(EmotionalActionProperties)) as EmotionalActionProperties;
            m_currentBehaviorCooldown = m_behaviorUpdateCooldown;
        }

        private void Start()
        {
            // TODO probably remove, scaling by weights can be done while searching for action
            // scale down the motivation gains by the weights given the personality
            //float[] motivationWeights = m_personalityManager.GetMotivationWeights();
            //foreach (MotivationActionProperties motivationActionProperties in m_motivationActionProperties)
            //{
            //    for (int i = 0; i < motivationWeights.Length; i++)
            //    {
            //        motivationActionProperties.motivationGain.m_motivationDesiresGain[i].m_value *= Mathf.Abs(motivationWeights[i]);
            //    }
            //}
        }

        private MotivationActionProperties ChooseMotivationAction()
        {
            MotivationActionProperties chosenAction = null;
            float[] currentDesires = null;
            float[] motivationWeights = null;
            float distance = 100.0f;
            int priority = -1;
            int mostSignificantMotivation = 0;
            float currentMostSignificantMotivationValue = 0.0f;
            if (m_usePersonalityModel)
            {
                currentDesires = m_personalityManager.GetCurrentDesires(ref mostSignificantMotivation, ref motivationWeights);
                //motivationWeights = m_personalityManager.GetMotivationWeights();
            }
            foreach (MotivationActionProperties motivationActionProperties in m_motivationActionProperties)
            {
                if (m_usePersonalityModel)
                {
                    float newDistance = 0.0f;
                    for (int i = 0; i < motivationActionProperties.motivationGain.m_motivationDesiresGain.Length; i++)
                    {
                        // for the gain that is too strong, but is of valid motivation type, dont punish that harshly
                        float gain = motivationActionProperties.motivationGain.m_motivationDesiresGain[i].m_value;
                        // OPTION A, double down the overreach
                        //if (currentDesires[i] > 0.0f && ((currentDesires[i] - gain < 0.0f)))
                        //{
                        //    float gainOverReach = -1.0f * (currentDesires[i] - gain);
                        //    gain -= gainOverReach / 2.0f;
                        //}
                        // OPTION B, employ weight given by personality traits
                        if (currentDesires[i] > 0.0f)
                        {
                            gain *= motivationWeights[i];

                            // OPTION 1, take into account only those that personality cares about
                            newDistance += Mathf.Abs(currentDesires[i] - gain);
                            Debug.Log(motivationActionProperties.name + " " + motivationActionProperties.motivationGain.m_motivationDesiresGain[i].m_motivationDesire.ToString() + " "
                                + currentDesires[i].ToString() + " " + gain.ToString() + " " + newDistance.ToString());
                        }

                        // OPTION 2, take into account every motivation gain
                        //newDistance += Mathf.Abs(currentDesires[i] - gain);
                        //Debug.Log(motivationActionProperties.name + " " + motivationActionProperties.motivationGain.m_motivationDesiresGain[i].m_motivationDesire.ToString() + " "
                        //    + currentDesires[i].ToString() + " " + newDistance.ToString());
                    }

                    float significance = motivationActionProperties.motivationGain.m_motivationDesiresGain[mostSignificantMotivation].m_value;

                    if ((
                        (newDistance < distance) 
                        || 
                        ((newDistance == distance) 
                        && (significance > currentMostSignificantMotivationValue))
                        ) 
                        && motivationActionProperties.CanBeTriggered())
                    {
                        currentMostSignificantMotivationValue = significance;
                        chosenAction = motivationActionProperties;
                        distance = newDistance;
                    }
                    continue;
                }

                if ((motivationActionProperties.priority > 0) && ((priority < 0) || (motivationActionProperties.priority < priority))
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
                m_cooldown += Time.deltaTime;
                if (m_usePersonalityModel && ActivateEmotionalAction())
                {
                    return;
                }
                if (m_cooldown >= m_currentBehaviorCooldown)
                {
                    m_cooldown = 0.0f;
                    m_currentBehaviorCooldown = m_behaviorUpdateCooldown;
                    ActivateMotivationAction();
                }
            }
        }
    }
}
