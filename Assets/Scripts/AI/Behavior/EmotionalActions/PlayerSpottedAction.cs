using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Personality.Emotions;


namespace AI.Behavior.EmotionalActions
{
    public class PlayerSpottedAction : MonoBehaviour
    {
        [SerializeField]
        private EmotionalActionEntry[] m_emotionTriggers;

        private List<Emotion> activeEmotions;

        private void Update()
        {
            foreach (EmotionalActionEntry emotionalEntry in m_emotionTriggers)
            {
                bool isTriggered = false;
                foreach (Emotion activeEmotion in activeEmotions)
                {
                    //if ((emotionalEntry.m_emotion.m_emotionType == activeEmotion.m_emotionType)
                    //    && emotionalEntry.m_emotion.m_initialIntensity >= activeEmotion.m_currentIntensity)
                    //{
                    //    emotionalEntry.m_action.SetActive(true);
                    //    isTriggered = true;
                    //    break;
                    //}
                }
                if (isTriggered)
                {
                    break;
                }
            }
        }
    }
}
