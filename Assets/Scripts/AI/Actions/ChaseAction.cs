using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player.EmptyClass;

namespace AI.Actions
{
    public class ChaseAction : MonoBehaviour
    {
        private Transform m_playerTransform;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;

        private bool m_isChasing = true;

        public bool isChasing
        {
            get { return m_isChasing; }
            set { m_isChasing = value; }
        }

        private void Awake()
        {
            m_navMeshAgent = transform.parent.transform.parent.GetComponent<NavMeshAgent>();
            m_animator = transform.parent.transform.parent.GetComponentInChildren<Animator>();
            m_playerTransform = FindObjectOfType<PlayerTagScript>().gameObject.transform;
        }

        private void OnEnable()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            StopCoroutine(Chase());
            StartCoroutine(Chase());
        }

        private IEnumerator Chase()
        {
            while (m_isChasing)
            {
                m_navMeshAgent.SetDestination(m_playerTransform.position);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}