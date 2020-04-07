using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;


namespace AI.Personality.Emotions
{
    public class EmotionManager
    {
        private float m_emotionIntensityLowerBound = 0.1f;

        private List<Emotion> m_activeEmotions = new List<Emotion>();
        private const float m_decayingConstant = 0.1f;

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
                float deltaTime = Time.time - m_activeEmotions[i].m_initialTime;
                deltaTime = deltaTime < 1.0f ? 1.0f : deltaTime;
                currentEmotion.m_currentIntensity 
                    = m_activeEmotions[i].m_initialIntensity 
                    * Mathf.Exp(-m_decayingConstant * deltaTime);
                m_activeEmotions[i] = currentEmotion;

                if (m_activeEmotions[i].m_currentIntensity <= 0.0f)
                {
                    m_activeEmotions.RemoveAt(i);
                }
            }
        }

        public Emotion[] AddEmotions(Emotion[] emotions, float[] currentMood, PersonalityModel personalityModel)
        {
            Emotion[] modifiedEmotions = new Emotion[emotions.Length];
            for (int i = 0; i< emotions.Length; i++)
            {
                Emotion newEmotion = emotions[i];
                newEmotion.m_initialIntensity
                = newEmotion.m_initialIntensity
                * (GetMoodInfluence(newEmotion.m_emotionType, currentMood)
                + GetPersonalityInfluence(newEmotion.m_emotionType, personalityModel)) / 2;
                newEmotion.m_currentIntensity = newEmotion.m_initialIntensity;
                newEmotion.m_initialTime = Time.time;
                newEmotion.m_emotionalTimeResponse = emotions[i].m_emotionalTimeResponse;
                if (newEmotion.m_currentIntensity > m_emotionIntensityLowerBound)
                {
                    m_activeEmotions.Add(newEmotion);
                }
                modifiedEmotions[i] = newEmotion;
            }
            return modifiedEmotions;
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
