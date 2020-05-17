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

        private EmotionalActionProperties m_emotionalActionProperties;

        private void Start()
        {
            m_emotionalActionProperties = GetComponent(typeof(EmotionalActionProperties)) as EmotionalActionProperties;

            foreach (EmotionalActionEntry emotionTrigger in m_emotionTriggers)
            {
                if (emotionTrigger.m_emotionType == m_emotionalActionProperties.triggeredEmotion)
                {
                    emotionTrigger.m_action.SetActive(true);
                    break;
                }
            }
        }
    }
}
