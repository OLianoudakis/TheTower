using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;

namespace AI.Personality
{
    public class MotivationManager
    {
        private float[] m_targetMotivation;
        private float[] m_baseMotivationWeights;
        private float[] m_currentMotivationWeights;
        private float[] m_currentFullfilment;
        private int m_mostSignificantMotivation;

        public MotivationManager(PersonalityModel personalityModel)
        {
            m_targetMotivation = new float[personalityModel.m_targetMotivations.Length];
            m_currentFullfilment = new float[personalityModel.m_targetMotivations.Length];
            m_baseMotivationWeights = new float[personalityModel.m_targetMotivations.Length];
            m_currentMotivationWeights = new float[personalityModel.m_targetMotivations.Length];
            float mostSignificantMotivationValue = 0.0f;
            for (int i = 0; i < m_targetMotivation.Length; i++)
            {
                m_targetMotivation[i] = personalityModel.m_targetMotivations[i].m_value;
                for (int j = 0; j < personalityModel.m_personalityTraitsValues.Length; j++)
                {
                    m_baseMotivationWeights[i] 
                        += ConstantMappings.MotivationToPersonalityTraits[i, j] 
                        * personalityModel.m_personalityTraitsValues[j].m_value;
                }
                m_currentFullfilment[i] = 0.0f;
                if ((m_targetMotivation[i] * m_baseMotivationWeights[i]) > mostSignificantMotivationValue)
                {
                    mostSignificantMotivationValue = m_targetMotivation[i];
                    m_mostSignificantMotivation = i;
                }
            }
        }

        public float[] GetCurrentDesires(
            List<Emotion> activeEmotions, 
            float emotionsIntensityLowerBound, 
            ref int mostSignificantMotivation,
            ref float[] motivationWeights)
        {
            float mostSignificantMotivationValue = 0.0f;
            float[] currentDesires = new float[16];
            motivationWeights = new float[16];
            for (int i = 0; i < currentDesires.Length; i++)
            {
                float weight = m_baseMotivationWeights[i];

                // update weight with emotion
                foreach (Emotion emotion in activeEmotions)
                {
                    bool isMapped = false;
                    foreach (MotivationDesires motivation in ConstantMappings.EmotionToMotivation[(int)emotion.m_emotionType])
                    {
                        if ((int)motivation == i)
                        {
                            isMapped = true;
                            weight *= 1.0f + (emotion.m_currentIntensity - emotionsIntensityLowerBound);
                        }
                    }
                    if (!isMapped)
                    {
                        weight *= 1.0f - (emotion.m_currentIntensity - emotionsIntensityLowerBound);
                    }
                }
                m_currentMotivationWeights[i] = weight;
                motivationWeights[i] = weight;
                currentDesires[i] = (m_targetMotivation[i] - m_currentFullfilment[i]) * weight;
                if (currentDesires[i] > mostSignificantMotivationValue)
                {
                    mostSignificantMotivationValue = currentDesires[i];
                    mostSignificantMotivation = i;
                }
            }
            // in a case every motivation is satisfied, just take the strongest target motivation
            if (mostSignificantMotivationValue == 0.0f)
            {
                mostSignificantMotivation = m_mostSignificantMotivation;
                mostSignificantMotivationValue = m_targetMotivation[m_mostSignificantMotivation];
            }

            return currentDesires;
        }

        public void UpdateCurrentMotivations(float[] motivationGain, float motivationGainRate, float motivationDecreaseRate)
        {
            if (motivationGain != null)
            {
                for (int i = 0; i < m_currentFullfilment.Length; i++)
                {
                    // dont add up to motivation if already full
                    if (m_currentFullfilment[i] < (m_targetMotivation[i] - 0.01f))
                    {
                        m_currentFullfilment[i] += (motivationGain[i] * m_currentMotivationWeights[i] * motivationGainRate);
                    }
                    // also decrease motivation over time (but not to negative values)
                    m_currentFullfilment[i] -= (motivationDecreaseRate * m_currentMotivationWeights[i] * motivationGainRate);
                    if (m_currentFullfilment[i] < 0.0f)
                    {
                        m_currentFullfilment[i] = 0.0f;
                    }
                }
            }
            else
            {
                // decrease current motivation even if theres no gain currently
                for (int i = 0; i < m_currentFullfilment.Length; i++)
                {
                    m_currentFullfilment[i] -= (motivationDecreaseRate * motivationGainRate);
                    if (m_currentFullfilment[i] < 0.0f)
                    {
                        m_currentFullfilment[i] = 0.0f;
                    }
                }
            }
        }
    }
}
