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
        private float[] m_currentDecreaseCooldown;
        private float m_decreaseCooldown;
        private int m_mostSignificantMotivation;
        private bool m_updateCurrentMotivation;

        public MotivationManager(PersonalityModel personalityModel, float decreaseCooldown, bool updateCurrentMotivation = false)
        {
            m_decreaseCooldown = decreaseCooldown;
            m_updateCurrentMotivation = updateCurrentMotivation;
            m_targetMotivation = new float[personalityModel.m_targetMotivations.Length];
            m_currentFullfilment = new float[personalityModel.m_targetMotivations.Length];
            m_baseMotivationWeights = new float[personalityModel.m_targetMotivations.Length];
            m_currentMotivationWeights = new float[personalityModel.m_targetMotivations.Length];
            m_currentDecreaseCooldown = new float[personalityModel.m_targetMotivations.Length];
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
                m_currentDecreaseCooldown[i] = 0.0f;
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
                if (m_updateCurrentMotivation)
                {
                    currentDesires[i] = (m_targetMotivation[i] - m_currentFullfilment[i]) * weight;
                }
                else
                {
                    currentDesires[i] = m_targetMotivation[i] * weight;
                }

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
            motivationWeights = m_currentMotivationWeights;
            return currentDesires;
        }

        public void UpdateCurrentMotivations(float[] motivationGain, float motivationGainRate, float motivationDecreaseRate)
        {
            if (m_updateCurrentMotivation)
            {
                if (motivationGain != null)
                {
                    for (int i = 0; i < m_currentFullfilment.Length; i++)
                    {
                        // only care about non-zero motivations
                        if (m_targetMotivation[i] != 0.0f)
                        {
                            // TODO this works only if target motivation is always >= 0.0f, to extend the model for negative values
                            // aditional case is needed

                            // dont add up to motivation if already full
                            if (motivationGain[i] > 0.0f)
                            {
                                if (m_currentFullfilment[i] < m_targetMotivation[i])
                                {
                                    m_currentDecreaseCooldown[i] = 0.0f;
                                    m_currentFullfilment[i] += (motivationGain[i] * m_currentMotivationWeights[i] * motivationGainRate);
                                }
                            }
                            else
                            {
                                // else, if the current fullfillment is not zero, reset it to zero after decrease cooldown time
                                if (m_currentFullfilment[i] > 0.0f)
                                {
                                    m_currentDecreaseCooldown[i] += motivationDecreaseRate;
                                    if (m_currentDecreaseCooldown[i] >= m_decreaseCooldown)
                                    {
                                        m_currentFullfilment[i] = 0.0f;
                                    }
                                    // OLD approach
                                    // else decrease motivation over time (but not to negative values), if current action has no gain for this motivation desire
                                    //m_currentFullfilment[i] -= (0.01f * m_currentMotivationWeights[i] * motivationGainRate);
                                    //if (m_currentFullfilment[i] < 0.0f)
                                    //{
                                    //    m_currentFullfilment[i] = 0.0f;
                                    //}
                                }
                            }
                        }
                    }
                }
                // TODO: for now, do nothing if no motivation gain attached, theres no situation of that happening so far
                //else
                //{
                //    // TODO this works only if target motivation is always >= 0.0f, to extend the model for negative values
                //    // aditional case is needed

                //    // decrease current motivation even if theres no gain currently
                //    for (int i = 0; i < m_currentFullfilment.Length; i++)
                //    {
                //        // only care about non-zero motivations
                //        if ((m_targetMotivation[i] != 0.0f))
                //        {
                //            m_currentFullfilment[i] -= (m_currentMotivationWeights[i] * motivationGainRate);
                //            if (m_currentFullfilment[i] < 0.0f)
                //            {
                //                m_currentFullfilment[i] = 0.0f;
                //            }
                //        }
                //    }
                //}
            }
        }
    }
}
