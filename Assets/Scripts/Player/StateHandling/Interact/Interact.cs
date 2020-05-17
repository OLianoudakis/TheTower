using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Environment;

namespace Player.StateHandling.Interact
{
    public class Interact : MonoBehaviour
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;
        private InteractibleDetector m_interactibleDetector;

        private void Awake()
        {
            m_interactibleDetector = FindObjectOfType(typeof(InteractibleDetector)) as InteractibleDetector;
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_agent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        }

        private void OnEnable()
        {
            m_animator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimIdle);

            if (!m_interactibleDetector)
            {
                m_interactibleDetector = FindObjectOfType(typeof(InteractibleDetector)) as InteractibleDetector;
            }
            if (m_interactibleDetector.interactible)
            {
                m_interactibleDetector.interactible.ActivateBehavior();
            }
            m_agent.isStopped = true;
        }
    }
}
