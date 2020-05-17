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

        private InteractibleDetector m_interactibleDetector;

        private Color c = Color.white;
        private Vector3 m_destination;

        public bool clickedOnEnable
        {
            get { return m_isClickedOnEnable; }
            set { m_isClickedOnEnable = value; }
        }

        private bool m_isClickedOnEnable = false;

        public void StopMoving()
        {
            m_agent.isStopped = true;
            m_agent.ResetPath();
        }
        
        private void Awake()
        {
            m_interactibleDetector = FindObjectOfType(typeof(InteractibleDetector)) as InteractibleDetector;
        }

        private void OnEnable()
        {
            m_isClickedOnEnable = true;
            //m_destination = m_interactibleDetector.interactible 
            //    ? m_interactibleDetector.interactible.interactiblePosition.position 
            //    : m_inputController.leftMouseClickPosition;
            //SetDestination();
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
            m_agent.SetDestination(m_destination);
            // TODO this would require to calculate path here, potentially too expensive
            //StopAtEdge(); // if we set the point beyond edge and currently standing on edge, do nothing
            if (m_positionMarker)
            {
                if (m_interactibleDetector.interactible)
                {
                    // dont show position marker if going to interactible place
                    m_positionMarker.transform.position = m_interactibleDetector.interactible.gameObject.transform.position + new Vector3(0.0f, 0.01f, 0.0f);
                    m_positionMarker.SetActive(false);
                }
                else
                {
                    m_positionMarker.transform.position = m_destination + new Vector3(0.0f, 0.01f, 0.0f);
                    m_positionMarker.SetActive(true);
                }
            }
        }

        private void StopAtEdge()
        {
            NavMeshHit navhit;
            if (!(NavMesh.FindClosestEdge(m_agent.nextPosition, out navhit, NavMesh.AllAreas) && (navhit.distance > 0.0f)))
            {
                m_agent.isStopped = true;
                m_agent.ResetPath();
                m_inputController.leftMouseClickPosition = transform.position; // switch the current destination to be current position
            }
        }

        private void SetPointOnEdge()
        {
            NavMeshHit navhit;
            if ((NavMesh.FindClosestEdge(m_destination, out navhit, NavMesh.AllAreas) && (navhit.distance < 0.3f)))
            {
                m_destination = navhit.position;
                if (m_interactibleDetector.interactible)
                {
                    m_interactibleDetector.interactible.interactiblePosition.position = m_destination;
                }
                else
                {
                    m_inputController.leftMouseClickPosition = m_destination;
                }
            }
        }

        private void Update()
        {
            // if new click while moving
            if (m_inputController.isLeftMouseClick || m_isClickedOnEnable)
            {
                m_agent.isStopped = false;
                m_animator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimWalk);
                m_isClickedOnEnable = false;
                m_destination = m_interactibleDetector.interactible
                    ? m_interactibleDetector.interactible.interactiblePosition.position
                    : m_inputController.leftMouseClickPosition;
                //SetPointOnEdge();
                SetDestination();
            }
            //else if (m_agent.hasPath)
            //{
            //    StopAtEdge();
            //}
            
        }

        void OnDrawGizmos()
        {
            if (m_agent)
            {
                if (m_agent.destination == null)
                {
                    return;
                }
                var path = new NavMeshPath();
                NavMesh.CalculatePath(transform.position, m_agent.destination, NavMesh.AllAreas, path);

                Gizmos.color = Color.red;
                Gizmos.DrawRay(transform.position, Vector3.up);
                Gizmos.DrawRay(m_agent.destination, Vector3.up);
                Gizmos.color = Color.green;
                var offset = 0.2f * Vector3.up;
                for (int i = 1; i < path.corners.Length; ++i)
                {
                    Gizmos.DrawLine(path.corners[i - 1] + offset, path.corners[i] + offset);
                }
            }
        }
    }
}
