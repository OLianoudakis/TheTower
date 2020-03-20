using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.EventSystems;

namespace AI.KnowledgeBase
{
    public class KnowledgeBase : MonoBehaviour
    {
        // private knowledge
        [SerializeField]
        private float m_playerStopFollowTime = 5.0f;

        [SerializeField]
        private float m_playerForgetTime = 20.0f;

        [SerializeField]
        private float m_noiseHeardForgetTime = 20.0f;

        private float m_currentNoiseForgetTime = 0.0f;
        private float m_currentPlayerForgetTime = 0.0f;
        private bool m_playerForgotten = true;

        // shared knowledge
        private bool m_playerHiding = false;
        private bool m_noiseHeard = false;
        private Vector3 m_noisePosition;
        private Vector3 m_lastPlayerPosition;
        private Transform m_playerTransform;

        public bool playerHiding
        {
            get { return m_playerHiding; }
        }

        public bool noiseHeard
        {
            get { return m_noiseHeard; }
        }

        public Vector3 noisePosition
        {
            get { return m_noisePosition; }
            set
            {
                m_noisePosition = value;
                m_noiseHeard = true;
                m_currentNoiseForgetTime = 0.0f;
            }
        }

        public Vector3 lastPlayerPosition
        {
            get { return m_lastPlayerPosition; }
            set
            {
                m_lastPlayerPosition = value;
                m_playerHiding = true;
                m_currentPlayerForgetTime = 0.0f;
            }
        }

        public Transform playerTransform
        {
            get { return m_playerTransform; }
            set
            {
                m_playerTransform = value;
                m_playerHiding = false;
                m_playerForgotten = false;
                m_currentPlayerForgetTime = 0.0f;
            }
        }

        public void PlayerSpotted(Transform playerTransform)
        {
            if (!m_playerTransform)
            {
                this.playerTransform = playerTransform;
                Events.Event playerSpottedevent;
                if (!m_playerForgotten)
                {   
                    playerSpottedevent.m_eventType = Events.EventType.PlayerSpotted;
                }
                else
                {
                    playerSpottedevent.m_eventType = Events.EventType.PlayerDiscovered;   
                }
                ExecuteEvents.Execute<ICustomEventTarget>(gameObject, null, (x, y) => x.ReceiveEvent(playerSpottedevent));
            }
        }

        private void ForgetNoiseHeard()
        {
            m_currentNoiseForgetTime += Time.deltaTime;
            if (m_playerHiding && (m_currentNoiseForgetTime >= m_noiseHeardForgetTime))
            {
                m_noiseHeard = false;
            }
        }

        private void ForgetPlayerSpotted()
        {
            m_currentPlayerForgetTime += Time.deltaTime;
            if (!m_playerHiding && (m_currentPlayerForgetTime >= m_playerStopFollowTime))
            {
                lastPlayerPosition = m_playerTransform.position;
                return;
            }
            if (m_playerHiding && (m_currentPlayerForgetTime >= m_playerForgetTime))
            {
                m_playerHiding = false;
                m_playerForgotten = true;
            }
        }

        private void Update()
        {
            if (!m_playerTransform && !m_playerForgotten)
            {
                ForgetPlayerSpotted();
            }
        }
    }
}
