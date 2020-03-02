using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace state
{
    namespace poiinspect
    {
        public class ToIdle : MonoBehaviour
        {
            [SerializeField]
            private int m_priority;

            [SerializeField]
            private POIDetector m_poiDetector;

            [SerializeField]
            private GameObject m_idleState;

            private TransitionHandler m_transitionHandler;
            private POIBehavior m_poiBehavior;

            private void OnEnable()
            {
                m_poiBehavior = m_poiDetector.poiBehavior;
            }

            private void Start()
            {
                m_transitionHandler = GetComponent(typeof(TransitionHandler)) as TransitionHandler;
            }

            // Update is called once per frame
            private void Update()
            {
                if (!m_poiBehavior.enabled || !m_poiBehavior.isActive)
                {
                    m_transitionHandler.AddActiveTransition(m_priority, m_idleState);
                }
            }
        }
    }
}
