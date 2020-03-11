using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Personality
{
    public class MotivationManager
    {
        private float[] m_motivationDesires;

        public MotivationManager()
        {
            m_motivationDesires = new float[16];
            for (int i = 0; i < m_motivationDesires.Length; i++)
            {
                m_motivationDesires[i] = 0.0f;
            }
        }
    }
}
