using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Player.StateHandling.Idle
{
    public class Idle : MonoBehaviour
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;

        private void Awake()
        {
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_agent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        }

        private void OnEnable()
        {
            m_animator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimIdle);
            m_agent.isStopped = true;
        }

        private void Start()
        {
            m_animator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimIdle);
        }
    }
}
