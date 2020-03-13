﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;


namespace AI.Personality.Emotions
{
    public class EmotionManager
    {
        [SerializeField]
        private float m_emotionIntensityLowerBound = 0.1f;

        private List<Emotion> m_activeEmotions = new List<Emotion>();
        private const float m_decayingConstant = 2.0f;

        public float emotionIntensityLowerBound
        {
            get { return m_emotionIntensityLowerBound; }
        }

        public List<Emotion> activeEmotions
        {
            get { return m_activeEmotions;}
        }

        public void DecayEmotionIntensity()
        {
            for (int i = m_activeEmotions.Count - 1; i >= 0; i--)
            {
                Emotion currentEmotion = m_activeEmotions[i];
                currentEmotion.m_currentIntensity 
                    = m_activeEmotions[i].m_initialIntensity 
                    * Mathf.Pow(Mathf.Epsilon, -m_decayingConstant * (Time.time - m_activeEmotions[i].m_initialTime));
                m_activeEmotions[i] = currentEmotion;

                if (m_activeEmotions[i].m_currentIntensity <= 0.0f)
                {
                    m_activeEmotions.RemoveAt(i);
                }
            }
        }

        public void AddEmotion(Emotion emotion, float[] currentMood, PersonalityModel personalityModel)
        {
            emotion.m_initialIntensity 
                = emotion.m_initialIntensity 
                * (GetMoodInfluence(emotion.m_emotionType, currentMood) 
                + GetPersonalityInfluence(emotion.m_emotionType, personalityModel)) / 2;
            emotion.m_currentIntensity = emotion.m_initialIntensity;
            emotion.m_initialTime = Time.time;
            m_activeEmotions.Add(emotion);
        }

        private float GetMoodInfluence(EmotionType emotionType, float[] currentMood)
        {
            int isInTheSameOctant = 1;
            int isInTheInverseOctant = 1;
            for (int i = 0; i < currentMood.Length; i++)
            {
                int discretizedCurrentMood = currentMood[i] >= 0.0f ? 1 : -1;
                int discretizedCurrentMoodInverted = discretizedCurrentMood * -1;
                int discretizedMoodByEmotion = ConstantMappings.MoodToEmotion[i, (int)emotionType] >= 0.0f ? 1 : -1;
                if (discretizedCurrentMood != discretizedMoodByEmotion)
                {
                    isInTheSameOctant = 0;
                    continue;
                }
                isInTheInverseOctant = 0;
            }

            return 1 + new Vector3(currentMood[0], currentMood[1], currentMood[2]).magnitude * (isInTheSameOctant - isInTheInverseOctant);
        }

        private float GetPersonalityInfluence(EmotionType emotionType, PersonalityModel personalityModel)
        {
            float influence = 0.0f;
            for(int i = 0; i < personalityModel.m_personalityTraitsValues.Length; i++)
            {
                influence +=
                    personalityModel.m_personalityTraitsValues[i].m_value
                    * ConstantMappings.EmotionToPersonalityTraits[(int)emotionType, i];
            }
            return influence;
        }
    }
}
