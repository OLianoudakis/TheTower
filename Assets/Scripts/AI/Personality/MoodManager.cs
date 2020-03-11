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

        public void UpdateMood()
        {

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

        private float CalculateEffectiveEmotion(MoodType moodType)
        {
            for (int i = 0; i < ConstantMappings.MoodToEmotion.GetLength(1); i++)
            {
                ConstantMappings.MoodToEmotion[(int)moodType, i]
            }
        }

        private float[] CalculateEffectiveMood(List<Emotion> activeEmotions)
        {
            float[] effectiveMood = new float[3];

            for (int i = 0; i < effectiveMood.Length; i++)
            {
                float effectiveEmotion = CalculateEffectiveEmotion((MoodType)i);
                effectiveMood[i] = m_baseMoodWeight * m_baseMood[i];
            }

            return effectiveMood;
        }
    }
}
