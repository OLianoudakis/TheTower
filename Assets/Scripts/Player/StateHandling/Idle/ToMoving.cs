using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.StateHandling.Idle
{
    public class ToMoving : MonoBehaviour
    {
        [SerializeField]
        private int m_priority;

        [SerializeField]
        private GameObject m_movingState;

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
            if (m_inputController.initialized && m_inputController.isLeftMouseClick
                && (Vector3.SqrMagnitude(m_inputController.leftMouseClickPosition - m_playerPosition.position) > MathConstants.SquaredDistance))
            {
                m_transitionHandler.AddActiveTransition(m_priority, m_movingState);
            }
        }
    }
}
