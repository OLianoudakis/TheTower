using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using POI;

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
        private POIDetector m_poiDetector;

        private Vector3 m_destination;

        private void OnEnable()
        {
            m_animator.SetInteger("AnimState", 1);
            m_destination = m_poiDetector.poiBehavior ? m_poiDetector.poiBehavior.GetPOIDestinationPoint() : m_inputController.leftMouseClickPosition;
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
                m_positionMarker.transform.position = m_poiDetector.poiBehavior
                ? m_poiDetector.poiBehavior.gameObject.transform.position + new Vector3(0.0f, 0.01f, 0.0f)
                : m_destination + new Vector3(0.0f, 0.01f, 0.0f);
            }
        }

        private void Update()
        {
            // if new click while moving
            if (m_inputController.isLeftMouseClick)
            {
                m_destination = m_poiDetector.poiBehavior ? m_poiDetector.poiBehavior.GetPOIDestinationPoint() : m_inputController.leftMouseClickPosition;
                SetDestination();
            }
        }
    }
}
