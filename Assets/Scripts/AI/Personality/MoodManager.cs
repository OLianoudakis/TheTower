using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Personality
{
    public class MoodManager
    {
        private float[] m_moods;

        public MoodManager()
        {
            m_moods = new float[3] { 0.0f, 0.0f, 0.0f };
        }
    }
}
