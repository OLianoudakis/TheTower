using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Personality
{
    public class EmotionManager
    {
        private float[] m_emotions;

        public EmotionManager()
        {
            m_emotions = new float[21];
            for (int i = 0; i < m_emotions.Length; i++)
            {
                m_emotions[i] = 0.0f;
            }
        }
    }
}
