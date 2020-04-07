using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.EventSystems;
using Environment;

namespace AI.KnowledgeBase
{
    // TODO: rework to sight memory and sound memory if time available to get rid of spaghetti
    public class KnowledgeBase : MonoBehaviour
    {
        [SerializeField]
        private bool m_useEvents = true;

        [SerializeField]
        private float m_playerStopFollowTime = 5.0f;

        [SerializeField]
        private float m_playerForgetTime = 20.0f;

        [SerializeField]
        private float m_environmentMovedForgetTime = 20.0f;

        [SerializeField]
        private float m_noiseHeardForgetTime = 20.0f;

        [SerializeField]
        private GameObject[] m_eventReceivers;

        private float m_currentNoiseForgetTime = 0.0f;
        private float m_currentPlayerForgetTime = 0.0f;
        private float m_currentEnvironmentMovedForgetTime = 0.0f;
        private bool m_playerForgotten = true;
        private bool m_noiseForgotten = true;
        private List<Movable> m_environmentObjects = new List<Movable>();
        private Dictionary<int, bool> m_environmentObjectsMap = new Dictionary<int, bool>();

        // shared knowledge
        private Transform m_playerTransform;
        private bool m_playerSuspicion = false;
        private bool m_playerHiding = false;
        private bool m_noiseHeard = false;
        private bool m_environmentObjectMoved = false;
        private Vector3 m_playerSuspicionPosition;
        private Vector3 m_noisePosition;
        private Vector3 m_lastKnownPlayerPosition;
        
        public bool playerSuspicion
        {
            get { return m_playerSuspicion; }
        }

        public bool playerHiding
        {
            get { return m_playerHiding; }
        }

        public bool noiseHeard
        {
            get { return m_noiseHeard; }
        }

        public bool environmentObjectMoved
        {
            get { return m_environmentObjectMoved; }
        }

        public void SetNoisePosition(Vector3 noisePosition)
        {
            m_noisePosition = noisePosition;
            m_noiseHeard = true;
            m_currentNoiseForgetTime = 0.0f;
            Events.Event noiseHeardEvent;
            noiseHeardEvent.m_eventType = Events.EventType.NoiseHeardBySomebodyElse;
            SendEvent(noiseHeardEvent);
        }

        public Vector3 GetLastKnownPlayerPosition()
        {
            return m_lastKnownPlayerPosition;
        }

        public Movable GetMovedObject()
        {
            if (m_environmentObjects.Count > 0)
            {
                return m_environmentObjects[0];
            }
            return null;
        }

        public Vector3 GetNoisePosition()
        {
            return m_noisePosition;
        }

        public Vector3 GetPlayerSuspicionPosition()
        {
            return m_playerSuspicionPosition;
        }

        public float GetPlayerStopFollowTime()
        {
            return m_playerStopFollowTime;
        }

        public Transform playerTransform
        {
            get { return m_playerTransform; }
            set
            {
                m_playerTransform = value;
                m_playerSuspicion = false;
                m_playerHiding = false;
                m_playerForgotten = false;
                m_currentPlayerForgetTime = 0.0f;
            }
        }

        public void EnvironmentObjectMoved(Movable environmentObject)
        {
            if (m_environmentObjects.Count == 0)
            {
                m_environmentObjectMoved = true;
                m_currentEnvironmentMovedForgetTime = 0.0f;
                Events.Event objectMovedEvent;
                objectMovedEvent.m_eventType = Events.EventType.EnvironmentObjectMoved;
                SendEvent(objectMovedEvent);
            }
            if (!m_environmentObjectsMap.ContainsKey(environmentObject.GetInstanceID()))
            {
                m_environmentObjects.Add(environmentObject);
                m_environmentObjectsMap.Add(environmentObject.GetInstanceID(), true);
            }
        }

        public void EnvironmentObjectPositionRestored(Movable environmentObject)
        {
            if (m_environmentObjectsMap.ContainsKey(environmentObject.GetInstanceID()))
            {
                m_environmentObjectsMap.Remove(environmentObject.GetInstanceID());
                m_environmentObjects.Remove(environmentObject);
                if (m_environmentObjects.Count == 0)
                {
                    m_environmentObjectMoved = false;
                }
            }
        }

        public void NoiseHeard(Vector3 noisePosition)
        {
            m_currentNoiseForgetTime = 0.0f;
            m_noiseHeard = true;
            m_noisePosition = noisePosition;
            Events.Event noiseHeardEvent;
            noiseHeardEvent.m_eventType = Events.EventType.NoiseHeard;
            SendEvent(noiseHeardEvent);
        }

        public void PlayerSpotted(Transform playerTransform)
        {
            if (m_playerTransform && (m_currentPlayerForgetTime < m_playerStopFollowTime))
            {
                // sight was lost just for a little while, restart the timer to stop follow
                m_currentPlayerForgetTime = 0.0f;
                return;
            }
            if (!m_playerTransform)
            {
                // player havent been seen for a while, send appropriate event
                // depending on whether totally forgotten or not
                Events.Event playerSpottedEvent;
                if (m_playerForgotten)
                {
                    playerSpottedEvent.m_eventType = Events.EventType.PlayerSpotted;
                }
                else
                {
                    playerSpottedEvent.m_eventType = Events.EventType.PlayerDiscovered;
                }
                SendEvent(playerSpottedEvent);
                this.playerTransform = playerTransform;
            }
        }

        public void PlayerSuspicion(Vector3 playerSuspicionPosition)
        {
            if (!m_playerTransform)
            {
                m_playerSuspicion = true;
                m_lastKnownPlayerPosition = playerSuspicionPosition;
                m_currentPlayerForgetTime = 0.0f;
                Events.Event playerSuspicionEvent;
                playerSuspicionEvent.m_eventType = Events.EventType.PlayerSuspicion;
                SendEvent(playerSuspicionEvent);
            }
        }

        private void ForgetEnvironmentObjectMoved()
        {
            m_currentEnvironmentMovedForgetTime += Time.deltaTime;
            if ((m_environmentObjects.Count > 0) && (m_currentEnvironmentMovedForgetTime >= m_environmentMovedForgetTime))
            {
                // remove the oldest object
                m_environmentObjectsMap.Remove(m_environmentObjects[0].GetInstanceID());
                m_environmentObjects.RemoveAt(0);
                m_currentEnvironmentMovedForgetTime = 0.0f;
                if (m_environmentObjects.Count == 0)
                {
                    m_environmentObjectMoved = false;
                }
            }
        }

        private void ForgetNoise()
        {
            m_currentNoiseForgetTime += Time.deltaTime;
            if (m_currentNoiseForgetTime >= m_noiseHeardForgetTime)
            {
                m_noiseHeard = false;
            }
        }

        private void ForgetPlayerSpotted()
        {
            m_currentPlayerForgetTime += Time.deltaTime;
            if (m_playerTransform && (m_currentPlayerForgetTime >= m_playerStopFollowTime))
            {
                SetLastKnownPlayerPosition(m_playerTransform.position);
                Events.Event playerLostEvent;
                playerLostEvent.m_eventType = Events.EventType.PlayerLost;
                SendEvent(playerLostEvent);
                return;
            }
            if ((m_playerHiding || m_playerSuspicion) && (m_currentPlayerForgetTime >= m_playerForgetTime))
            {
                m_playerHiding = false;
                m_playerSuspicion = false;
                m_playerForgotten = true;
            }
        }

        private void SendEvent(Events.Event eventToSend)
        {
            if (m_useEvents)
            {
                foreach (GameObject receiver in m_eventReceivers)
                {
                    ExecuteEvents.Execute<ICustomEventTarget>(receiver, null, (x, y) => x.ReceiveEvent(eventToSend));
                }
            }
        }

        public void SetLastKnownPlayerPosition(Vector3 lastKnownPlayerPosition)
        {
            m_playerTransform = null;
            m_lastKnownPlayerPosition = lastKnownPlayerPosition;
            m_playerHiding = true;
            m_currentPlayerForgetTime = 0.0f;
        }

        public void SetPlayerStopFollowTime(float playerStopFollowTime)
        {
            m_playerStopFollowTime = playerStopFollowTime;
        }

        private void Update()
        {
            if (!m_playerForgotten)
            {
                ForgetPlayerSpotted();
            }
            if (m_environmentObjectMoved)
            {
                ForgetEnvironmentObjectMoved();
            }
            if (m_noiseHeard)
            {
                ForgetNoise();
            }
        }
    }
}
