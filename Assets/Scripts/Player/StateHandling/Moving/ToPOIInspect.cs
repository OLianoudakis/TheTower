using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using POI;

namespace Player.StateHandling.Moving
{
    public class ToPOIInspect : MonoBehaviour
    {
        [SerializeField]
        private int m_priority;

        [SerializeField]
        private GameObject m_poiInspectState;

        [SerializeField]
        private InputController m_inputController;

        [SerializeField]
        private Transform m_playerPosition;

        [SerializeField]
        private POIDetector m_poiDetector;

        private TransitionHandler m_transitionHandler;

        private void Start()
        {
            m_transitionHandler = GetComponent(typeof(TransitionHandler)) as TransitionHandler;
        }
        private void Update()
        {
            Vector3 destination = m_poiDetector.poiBehavior ? m_poiDetector.poiBehavior.GetPOIDestinationPoint() : m_inputController.leftMouseClickPosition;
            // take only x and z coords, cause y for player is always bit higher
            if ((Vector3.SqrMagnitude(new Vector3(destination.x, 0.0f, destination.z)
                - new Vector3(m_playerPosition.position.x, 0.0f, m_playerPosition.position.z))
                < Constants.SquaredDistance)
                && m_poiDetector.poiBehavior && m_poiDetector.poiBehavior.enabled)
            {
                m_transitionHandler.AddActiveTransition(m_priority, m_poiInspectState);
            }
        }
    }
}
