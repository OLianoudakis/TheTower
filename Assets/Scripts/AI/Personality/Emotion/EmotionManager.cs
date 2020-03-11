using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Personality.Emotions
{
    public class EmotionManager
    {
        private List<Emotion> m_activeEmotions = new List<Emotion>();

        public List<Emotion> activeEmotions
        {
            get { return m_activeEmotions; }
        }
    }
}
