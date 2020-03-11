using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;

namespace AI.Personality
{
    public class MoodManager
    {
        private float[] m_mood;
        private float[] m_baseMood;
        private float m_baseMoodWeight = 0.5f;

        public MoodManager(PersonalityModel personalityModel)
        {
            m_mood = new float[3] { 0.0f, 0.0f, 0.0f };
            m_baseMood = new float[3] { 0.0f, 0.0f, 0.0f };
            CalculateBaseMood(personalityModel);
        }

        public void UpdateMood(List<Emotion> activeEmotions)
        {
            float[] effectiveMood = CalculateEffectiveMood(activeEmotions);
            for (int i = 0; i < m_mood.Length; i++)
            {
                m_mood[i] = ((1.0f - m_baseMoodWeight) * m_baseMood[i]) + (m_baseMoodWeight * effectiveMood[i]);
            }
        }

        private void CalculateBaseMood(PersonalityModel personalityModel)
        {
            for (int i = 0; i < m_baseMood.Length; i++)
            {
                for (int j = 0; j < personalityModel.m_personalityTraitsValues.Length; j++)
                {
                    m_baseMood[i] += ConstantMappings.MoodToPersonalityTraits[i,j] * personalityModel.m_personalityTraitsValues[j].m_value;
                }
            }
        }

        
        private float CalculateEffectiveEmotion(MoodType moodType, List<Emotion> activeEmotions)
        {
            float effectiveEmotion = 0.0f;
            for (int i = 0; i < ConstantMappings.MoodToEmotion.GetLength(1); i++)
            {
                effectiveEmotion += 
                    ConstantMappings.MoodToEmotion[(int)moodType, i] 
                    * Mathf.Min(1.0f, CalculateEmotionIntensity(activeEmotions, (EmotionType)i));
            }
            return effectiveEmotion;
        }

        private float[] CalculateEffectiveMood(List<Emotion> activeEmotions)
        {
            float[] effectiveMood = new float[3];

            for (int i = 0; i < effectiveMood.Length; i++)
            {
                float effectiveEmotion = CalculateEffectiveEmotion((MoodType)i, activeEmotions);
                effectiveMood[i] = m_baseMoodWeight * m_baseMood[i] + effectiveEmotion;
            }

            return effectiveMood;
        }

        private float CalculateEmotionIntensity(List<Emotion> activeEmotions, EmotionType emotionType)
        {
            float emotionIntensity = 0.0f;
            foreach (Emotion emotion in activeEmotions)
            {
                if (emotion.m_emotionType == emotionType)
                {
                    emotionIntensity += emotion.m_value;
                }
            }
            return emotionIntensity;
        }

    }
}
