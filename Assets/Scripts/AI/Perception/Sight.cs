using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Perception
{
    public class Sight : MonoBehaviour
    {
        [SerializeField]
        private float m_fieldOfViewDeg = 45.0f;

        [SerializeField]
        private float m_distance = 10.0f;

        private Transform m_playerTransform;
        private Transform m_myTransform;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;

        private void Start()
        {
            SharedAI sharedAI = FindObjectOfType(typeof(SharedAI)) as SharedAI;
            if (sharedAI)
            {
                m_playerTransform = sharedAI.playerTransform;
            }
            m_myTransform = GetComponent(typeof(Transform)) as Transform;
            m_knowledgeBase = GetComponent(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void Update()
        {
            Vector3 directionToEnemy = (m_playerTransform.position - m_myTransform.position);
            if (directionToEnemy.magnitude <= m_distance)
            {
                float angle = Vector3.Angle(m_myTransform.forward, directionToEnemy.normalized);
                if (angle < m_fieldOfViewDeg)
                {
                    if (!m_knowledgeBase.playerTransform)
                    {
                        m_knowledgeBase.PlayerSpotted(m_playerTransform);
                    }
                }
                else if(m_knowledgeBase.playerTransform)
                {
                    m_knowledgeBase.playerTransform = null;
                }
            }
        }
    }
}
