using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.StateHandling.Crouching
{
    public class ToCrouching : MonoBehaviour
    {
        public bool playerCanHide
        {
            get { return m_playerCanHide; }
            set { m_playerCanHide = value; }
        }
        private bool m_playerCanHide;

        [SerializeField]
        private int m_priority;

        [SerializeField]
        private GameObject m_crouchingState;

        [SerializeField]
        private InputController m_inputController;

        private TransitionHandler m_transitionHandler;

        private void Awake()
        {
            m_transitionHandler = GetComponent(typeof(TransitionHandler)) as TransitionHandler;
        }

        private void Start()
        {
            m_playerCanHide = false;
        }

        private void Update()
        {
            if (m_inputController.initialized && m_inputController.isRightMouseClick && m_playerCanHide)
            {
                m_transitionHandler.AddActiveTransition(m_priority, m_crouchingState);
            }
        }
    }
}
