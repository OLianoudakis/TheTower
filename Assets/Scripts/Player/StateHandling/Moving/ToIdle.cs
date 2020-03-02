using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace state
{
    namespace moving
    {
        public class ToIdle : MonoBehaviour
        {
            [SerializeField]
            private int m_priority;

            [SerializeField]
            private GameObject m_idleState;

            [SerializeField]
            private InputController m_inputController;

            [SerializeField]
            private Transform m_playerPosition;

            private TransitionHandler m_transitionHandler;

            private void Start()
            {
                m_transitionHandler = GetComponent(typeof(TransitionHandler)) as TransitionHandler;
            }

            private void Update()
            {
                if (Vector3.SqrMagnitude(m_inputController.leftMouseClickPosition - m_playerPosition.position) < Constants.SquaredDistance)
                {
                    m_transitionHandler.AddActiveTransition(m_priority, m_idleState);
                }
            }
        }
    }
}