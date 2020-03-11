using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Personality
{
    public class PersonalityManager : MonoBehaviour
    {
        [SerializeField]
        private PersonalityModel m_personalityModel;

        // TODO make scriptable object for event emotion intensity that contains mappings of events to specific emotion and their intensity
        //[SerializeField]
        //private EventEmotionIntensity

        private EmotionManager m_emotionManager = new EmotionManager();
        private MoodManager m_moodManager = new MoodManager();
        private MotivationManager m_motivationManager = new MotivationManager();

        private void Start()
        {

        }

        private void Update()
        {
            // TODO update emotions, mood and current motivation
        }
    }
}
