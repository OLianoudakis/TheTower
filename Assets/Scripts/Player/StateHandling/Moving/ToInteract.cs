using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Player.StateHandling.Moving
{
    public class ToInteract : MonoBehaviour
    {
        [SerializeField]
        private int m_priority;

        [SerializeField]
        private GameObject m_interactState;

        [SerializeField]
        private Transform m_playerPosition;

        [SerializeField]
        private InteractibleDetector m_interactibleDetector;

        private TransitionHandler m_transitionHandler;

        private void Start()
        {
            m_transitionHandler = GetComponent(typeof(TransitionHandler)) as TransitionHandler;
        }
        private void Update()
        {
            if (m_interactibleDetector.interactible && m_interactibleDetector.interactible.enabled)
            {
                Vector3 destination = m_interactibleDetector.interactible.interactiblePosition.position;
                // take only x and z coords, cause y for player is always bit higher
                if (Vector3.SqrMagnitude(new Vector3(destination.x, 0.0f, destination.z)
                - new Vector3(m_playerPosition.position.x, 0.0f, m_playerPosition.position.z)) < Constants.SquaredDistance)
                {
                    m_transitionHandler.AddActiveTransition(m_priority, m_interactState);
                }
            }
        }
    }
}
