using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;

namespace AI.Personality
{
    public class MotivationManager
    {
        private float[] m_targetMotivation;
        private float[] m_currentFullfilment;
        private int m_mostSignificantMotivation;

        public MotivationManager(PersonalityModel personalityModel)
        {
            m_targetMotivation = new float[16];
            m_currentFullfilment = new float[16];
            float mostSignificantMotivationValue = 0.0f;
            for (int i = 0; i < m_targetMotivation.Length; i++)
            {
                for (int j = 0; j < personalityModel.m_personalityTraitsValues.Length; j++)
                {
                    m_targetMotivation[i] 
                        += ConstantMappings.MotivationToPersonalityTraits[i, j] 
                        * personalityModel.m_personalityTraitsValues[j].m_value;
                }
                if (m_targetMotivation[i] < 0.0f)
                {
                    m_targetMotivation[i] = 0.0f;
                }
                m_currentFullfilment[i] = 0.0f;
                if (m_targetMotivation[i] > mostSignificantMotivationValue)
                {
                    mostSignificantMotivationValue = m_targetMotivation[i];
                    m_mostSignificantMotivation = i;
                }
            }
        }

        public float[] GetCurrentDesires(List<Emotion> activeEmotions, float emotionsIntensityLowerBound, ref int mostSignificantMotivation)
        {
            mostSignificantMotivation = m_mostSignificantMotivation;
            float[] currentDesires = new float[16];
            for (int i = 0; i < currentDesires.Length; i++)
            {
                // for now not using pow to return negative desire (not distance, but rather a value)
                //currentDesires[i] = Mathf.Pow((m_targetMotivation[i] - m_currentFullfilment[i]), 2.0f) * ; 
                float weight = m_targetMotivation[i];

                // update weight with emotion
                foreach (Emotion emotion in activeEmotions)
                {
                    bool isMapped = false;
                    foreach (MotivationDesires motivation in ConstantMappings.EmotionToMotivation[(int)emotion.m_emotionType])
                    {
                        if ((int)motivation == i)
                        {
                            isMapped = true;
                            weight *= 1 + (emotion.m_currentIntensity - emotionsIntensityLowerBound);
                        }
                    }
                    if (!isMapped)
                    {
                        weight -= 1 - (emotion.m_currentIntensity - emotionsIntensityLowerBound);
                    }
                }
                if (weight < 0.0f)
                {
                    weight = 0.0f;
                }
                currentDesires[i] = (m_targetMotivation[i] - m_currentFullfilment[i]) * weight;
            }
            return currentDesires;
        }

        public void UpdateCurrentMotivations(float[] motivationGain, float motivationGainRate, float motivationDecreaseRate)
        {
            if (motivationGain != null)
            {
                for (int i = 0; i < m_currentFullfilment.Length; i++)
                {
                    m_currentFullfilment[i] += (motivationGain[i] * motivationGainRate) - (motivationDecreaseRate * motivationGainRate);
                    if (m_currentFullfilment[i] > m_targetMotivation[i])
                    {
                        m_currentFullfilment[i] = m_targetMotivation[i];
                    }
                    if (m_currentFullfilment[i] < 0.0f)
                    {
                        m_currentFullfilment[i] = 0.0f;
                    }
                }
            }
        }
    }
}
