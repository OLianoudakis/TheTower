using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player.EmptyClass;

namespace AI.Behavior.MotivationActions.Actions
{
    public class ChaseAction : MonoBehaviour
    {
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private MotivationActionProperties m_motivationActionProperties;

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
            m_motivationActionProperties = GetComponent(typeof(MotivationActionProperties)) as MotivationActionProperties;
        }

        private void OnEnable()
        {
            m_navMeshAgent.isStopped = false;
            m_isChasing = true;
            m_motivationActionProperties.canInterrupt = false;
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            StopCoroutine(Chase());
            StartCoroutine(Chase());
        }

        private void OnDisable()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            m_navMeshAgent.isStopped = true;
            m_navMeshAgent.ResetPath();
            m_isChasing = false;
        }

        private IEnumerator Chase()
        {
            yield return new WaitForEndOfFrame();
            while (m_isChasing)
            {
                if (m_knowledgeBase.playerTransform)
                {
                    m_navMeshAgent.SetDestination(m_knowledgeBase.playerTransform.position);
                    if (!m_navMeshAgent.hasPath)
                    {
                        NavMeshHit hit;
                        NavMesh.SamplePosition(m_knowledgeBase.playerTransform.position, out hit, 1.0f, NavMesh.AllAreas);
                        m_navMeshAgent.SetDestination(hit.position);
                    }
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    m_isChasing = false;
                    m_motivationActionProperties.canInterrupt = true;
                }
            }
        }
    }
}