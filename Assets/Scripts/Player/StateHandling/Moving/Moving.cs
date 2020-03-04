using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Environment;

namespace Player.StateHandling.Moving
{
    public class Moving : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private GameObject m_positionMarker;

        [SerializeField]
        private InputController m_inputController;

        [SerializeField]
        private NavMeshAgent m_agent;

        [SerializeField]
        private InteractibleDetector m_interactibleDetector;

        private Vector3 m_destination;

        private void OnEnable()
        {
            m_animator.SetInteger("AnimState", 1);
            m_destination = m_interactibleDetector.interactible 
                ? m_interactibleDetector.interactible.interactiblePosition.position 
                : m_inputController.leftMouseClickPosition;
            SetDestination();
        }

        private void OnDisable()
        {
            if (m_positionMarker)
            {
                m_positionMarker.SetActive(false);
            }
        }

        private void SetDestination()
        {
            m_agent.destination = m_destination;
            if (m_positionMarker)
            {
                if (!m_positionMarker.activeInHierarchy)
                {
                    m_positionMarker.SetActive(true);
                }
                m_positionMarker.transform.position = m_interactibleDetector.interactible
                ? m_interactibleDetector.interactible.gameObject.transform.position + new Vector3(0.0f, 0.01f, 0.0f)
                : m_destination + new Vector3(0.0f, 0.01f, 0.0f);
            }
        }

        private void Update()
        {
            // if new click while moving
            if (m_inputController.isLeftMouseClick)
            {
                m_destination = m_interactibleDetector.interactible 
                    ? m_interactibleDetector.interactible.interactiblePosition.position 
                    : m_inputController.leftMouseClickPosition;
                SetDestination();
            }
        }
    }
}
