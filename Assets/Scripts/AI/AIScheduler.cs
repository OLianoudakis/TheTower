using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.Behavior;
using AI.Personality;

namespace AI
{
    public class AIScheduler : MonoBehaviour
    {
        [SerializeField]
        private float m_behaviorUpdatePeriod = 0.5f;

        [SerializeField]
        private float m_personalityUpdatePeriod = 0.1f;

        private BehaviorManager[] m_behaviorManagers;
        private PersonalityManager[] m_personalityManagers;
        private float m_behaviorUpdateInterval;
        private float m_personalityUpdateInterval;
        private float m_behaviorUpdateTimer = 0.0f;
        private float m_personalityUpdateTimer = 0.0f;
        private int m_behaviorUpdateIndex = 0;
        private int m_personalityUpdateIndex = 0;

        private void Awake()
        {
            m_behaviorManagers = GetComponentsInChildren<BehaviorManager>();
            m_personalityManagers = GetComponentsInChildren<PersonalityManager>();
            m_behaviorUpdateInterval = m_behaviorUpdatePeriod / m_behaviorManagers.Length;
            m_personalityUpdateInterval = m_personalityUpdatePeriod / m_personalityManagers.Length;
        }

        private void Update()
        {
            // TEST
            //m_behaviorManagers[0].UpdateBehavior();
            //m_personalityManagers[0].UpdatePersonality(Time.deltaTime);
            // Update scheduled behaviors
            //m_behaviorUpdateTimer += Time.deltaTime;
            //float updateFraction =  m_behaviorUpdateTimer / m_behaviorUpdateInterval;
            //int updateAmount = (int)updateFraction;
            //int updateAmountTmp = updateAmount;
            //int i = m_behaviorUpdateIndex;
            //for (; i < m_behaviorUpdateIndex + updateAmountTmp; i++)
            //{
            //    if (i >= m_behaviorManagers.Length)
            //    {
            //        updateAmountTmp = updateAmountTmp - i;
            //        m_behaviorUpdateIndex = 0;
            //        i = 0;
            //    }
            //    m_behaviorManagers[i].UpdateBehavior();
            //}
            //Debug.Log(updateAmount);
            //m_behaviorUpdateIndex = i;
            //m_behaviorUpdateTimer = m_behaviorUpdateTimer - ((float)updateAmount * m_behaviorUpdateInterval);

            //// Update scheduled personalities
            //m_personalityUpdateTimer += Time.deltaTime;
            //updateFraction = m_personalityUpdateTimer / m_personalityUpdateInterval;
            //updateAmount = (int)updateFraction;
            //updateAmountTmp = updateAmount;
            //for (i = m_personalityUpdateIndex; i < m_personalityUpdateIndex + updateAmountTmp; i++)
            //{
            //    if (i >= m_personalityManagers.Length)
            //    {
            //        updateAmountTmp = updateAmountTmp - i;
            //        m_personalityUpdateIndex = 0;
            //        i = 0;
            //    }
            //    m_personalityManagers[i].UpdatePersonality(m_personalityUpdatePeriod);
            //}
            //m_personalityUpdateIndex = i;
            //m_personalityUpdateTimer = m_personalityUpdateTimer - ((float)updateAmount * m_personalityUpdateInterval);
        }
    }
}
