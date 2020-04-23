using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Player.StateHandling.Crouching
{
    public class Crouching : MonoBehaviour
    {
        private Animator m_animator;
        private NavMeshAgent m_agent;

        private CapsuleCollider m_playerCollider;
        private int m_playerLayer;

        private void Awake()
        {
            m_agent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_playerCollider = transform.parent.parent.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            m_playerLayer = m_playerCollider.gameObject.layer;
        }

        private void OnEnable()
        {
            m_agent.isStopped = true;
            //m_agent.enabled = false;
            m_animator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimCrouch);
            m_playerCollider.center = new Vector3(0.0f, 0.38f, 0.0f);
            m_playerCollider.height = 1.76f;
            //m_playerCollider.gameObject.layer = LayerMask.NameToLayer("Hiding");
        }

        private void OnDisable()
        {
            m_animator.SetInteger(AnimationConstants.AnimationState, AnimationConstants.AnimIdle);
            m_playerCollider.center = new Vector3(0.0f, 1.0f, 0.0f);
            m_playerCollider.height = 3.0f;
            //m_playerCollider.gameObject.layer = m_playerLayer;
        }
    }
}
