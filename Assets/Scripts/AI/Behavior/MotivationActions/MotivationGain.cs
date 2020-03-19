using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AI.Personality;

namespace AI.Behavior.MotivationActions
{
    [Serializable]
    public struct MotivationGainEntry
    {
        public MotivationDesires m_motivationDesire;
        public float m_value;

        public MotivationGainEntry(MotivationDesires motivationDesire, float value)
        {
            m_motivationDesire = motivationDesire;
            m_value = value;
        }
    }

    [CreateAssetMenu(fileName = "New Motivation Gain", menuName = "Motivation Gain")]
    public class MotivationGain : ScriptableObject
    {
        [SerializeField]
        public MotivationGainEntry[] m_motivationDesiresGain
            = new MotivationGainEntry[16]
            {
                new MotivationGainEntry(MotivationDesires.Acceptance, 0.0f),
                new MotivationGainEntry(MotivationDesires.Curiosity, 0.0f),
                new MotivationGainEntry(MotivationDesires.Eating, 0.0f),
                new MotivationGainEntry(MotivationDesires.Family, 0.0f),
                new MotivationGainEntry(MotivationDesires.Honor, 0.0f),
                new MotivationGainEntry(MotivationDesires.Idealism, 0.0f),
                new MotivationGainEntry(MotivationDesires.Independence, 0.0f),
                new MotivationGainEntry(MotivationDesires.Order, 0.0f),
                new MotivationGainEntry(MotivationDesires.PhysicalActivity, 0.0f),
                new MotivationGainEntry(MotivationDesires.Power, 0.0f),
                new MotivationGainEntry(MotivationDesires.Romance, 0.0f),
                new MotivationGainEntry(MotivationDesires.Saving, 0.0f),
                new MotivationGainEntry(MotivationDesires.SocialContact, 0.0f),
                new MotivationGainEntry(MotivationDesires.SocialStatus, 0.0f),
                new MotivationGainEntry(MotivationDesires.Tranquility, 0.0f),
                new MotivationGainEntry(MotivationDesires.Vengeance, 0.0f)
            };

        private float[] m_gainAsArray;

        public float[] gainAsArray
        {
            get { return m_gainAsArray; }
        }

        private void Awake()
        {
            m_gainAsArray = new float[m_motivationDesiresGain.Length];
            for (int i = 0; i < m_motivationDesiresGain.Length; i++)
            {
                m_gainAsArray[i] = m_motivationDesiresGain[i].m_value;
            }
        }
    }
}
