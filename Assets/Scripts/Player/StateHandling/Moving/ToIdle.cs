using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Player.StateHandling.Moving
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

        private InteractibleDetector m_interactibleDetector;
        private TransitionHandler m_transitionHandler;

        private void Start()
        {
            m_transitionHandler = GetComponent(typeof(TransitionHandler)) as TransitionHandler;
            m_interactibleDetector = FindObjectOfType(typeof(InteractibleDetector)) as InteractibleDetector;
        }

        private void Update()
        {
            Vector3 destination = m_interactibleDetector.interactible 
                ? m_interactibleDetector.interactible.interactiblePosition.position
                : m_inputController.leftMouseClickPosition;
            if ((Vector3.SqrMagnitude(new Vector3(destination.x, 0.0f, destination.z) 
                - new Vector3(m_playerPosition.position.x, 0.0f, m_playerPosition.position.z)) < MathConstants.SquaredDistance))
            {
                m_transitionHandler.AddActiveTransition(m_priority, m_idleState);
            }
        }
    }
}