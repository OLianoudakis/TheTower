using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

namespace AI.KnowledgeBase
{
    public class KnowledgeBase : MonoBehaviour, ICustomEventTarget
    {
        private bool m_changeHappened = false;

        private Transform m_playerTransform;

        public Transform playerTransform
        {
            get { return m_playerTransform; }
            set { m_playerTransform = value; m_changeHappened = true; }
        }

        public void ReceiveEvent(Events.Event receivedEvent)
        {

        }

        private void ForgetKnowledge()
        {

        }

        private void Update()
        {
            if (m_changeHappened)
            {
                // TODO: share the knowledgebase on change
                m_changeHappened = false;
            }

            // TODO: forget values after a while
        }
    }
}
