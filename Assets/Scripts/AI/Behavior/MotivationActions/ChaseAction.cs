using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player.EmptyClass;

namespace AI.Actions
{
    public class ChaseAction : MonoBehaviour
    {
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;

        private bool m_isChasing = true;

        public bool isChasing
        {
            get { return m_isChasing; }
            set { m_isChasing = value; }
        }

        private void Awake()
        {
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnEnable()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            StopCoroutine(Chase());
            StartCoroutine(Chase());
        }

        private IEnumerator Chase()
        {
            yield return new WaitForEndOfFrame();
            while (m_isChasing)
            {
                if (m_knowledgeBase.playerTransform)
                {
                    m_navMeshAgent.SetDestination(m_knowledgeBase.playerTransform.position);
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    m_isChasing = false;
                }
            }
        }
    }
}