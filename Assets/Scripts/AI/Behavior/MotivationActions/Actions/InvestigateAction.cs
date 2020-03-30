using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Behavior.MotivationActions.Actions
{
    public class InvestigateAction : MonoBehaviour
    {
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private Vector3 m_sourceOfSuspicion = Vector3.zero;

        public void SetSuspicionSource(Vector3 source)
        {
            m_sourceOfSuspicion = source;
        }

        private void Awake()
        {
            m_animator = transform.parent.transform.parent.GetComponentInChildren<Animator>();
            m_navMeshAgent = transform.parent.transform.parent.GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            if (m_navMeshAgent.velocity.magnitude <= 0.0f)
            {
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerLookAround);
            }
            else
            {
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            }
        }

        private void OnEnable()
        {
            m_navMeshAgent.SetDestination(m_sourceOfSuspicion);
        }
    }
}