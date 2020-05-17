using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AI.Personality;


namespace AI.Behavior.MotivationActions
{
    [CreateAssetMenu(fileName = "New Motivation Gain", menuName = "Motivation Gain")]
    public class MotivationGain : ScriptableObject
    {
        [SerializeField]
        public MotivationValueEntry[] m_motivationDesiresGain
            = new MotivationValueEntry[16]
            {
                new MotivationValueEntry(MotivationDesires.Acceptance, 0.0f),
                new MotivationValueEntry(MotivationDesires.Curiosity, 0.0f),
                new MotivationValueEntry(MotivationDesires.Eating, 0.0f),
                new MotivationValueEntry(MotivationDesires.Family, 0.0f),
                new MotivationValueEntry(MotivationDesires.Honor, 0.0f),
                new MotivationValueEntry(MotivationDesires.Idealism, 0.0f),
                new MotivationValueEntry(MotivationDesires.Independence, 0.0f),
                new MotivationValueEntry(MotivationDesires.Order, 0.0f),
                new MotivationValueEntry(MotivationDesires.PhysicalActivity, 0.0f),
                new MotivationValueEntry(MotivationDesires.Power, 0.0f),
                new MotivationValueEntry(MotivationDesires.Romance, 0.0f),
                new MotivationValueEntry(MotivationDesires.Saving, 0.0f),
                new MotivationValueEntry(MotivationDesires.SocialContact, 0.0f),
                new MotivationValueEntry(MotivationDesires.SocialStatus, 0.0f),
                new MotivationValueEntry(MotivationDesires.Tranquility, 0.0f),
                new MotivationValueEntry(MotivationDesires.Vengeance, 0.0f)
            };

        private float[] m_gainAsArray = null;

        public float[] gainAsArray
        {
            get
            {
                // TODO somehow this doesnt work, the m_gainAsArray gets miraculously initialized
                // for now everytime this gets called the array is transformed, this should be refactored if possible
                //if (m_gainAsArray != null)
                //{
                //    return m_gainAsArray;
                //}
                float[] gainAsArray = new float[m_motivationDesiresGain.Length];
                for (int i = 0; i < m_motivationDesiresGain.Length; i++)
                {
                    gainAsArray[i] = m_motivationDesiresGain[i].m_value;
                }
                return gainAsArray;
            }
        }
    }
}
