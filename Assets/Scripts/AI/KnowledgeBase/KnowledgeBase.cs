using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.KnowledgeBase
{
    public class KnowledgeBase : MonoBehaviour
    {
        private bool m_changeHappened = false;

        private Transform m_playerTransform;

        public Transform playerTransform
        {
            get { return m_playerTransform; }
            set { m_playerTransform = value; m_changeHappened = true; }
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
