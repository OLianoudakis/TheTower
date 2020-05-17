using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player.EmptyClass;

namespace AI.Behavior.MotivationActions.Actions
{
    public class ChaseAction : MonoBehaviour
    {
        [SerializeField]
        private float m_moveSpeed = 3.5f;

        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private MotivationActionProperties m_motivationActionProperties;
        private float m_cooldown = 0.0f;
        private float m_previousMoveSpeed;

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
            //m_motivationActionProperties.canInterrupt = false;
            m_previousMoveSpeed = m_navMeshAgent.speed;
            m_navMeshAgent.speed = m_moveSpeed;
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
        }

        private void OnDisable()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            m_navMeshAgent.isStopped = true;
            m_motivationActionProperties.canInterrupt = true;
            m_navMeshAgent.speed = m_previousMoveSpeed;
            m_navMeshAgent.ResetPath();
        }

        private void Update()
        {
            m_cooldown += Time.deltaTime;
            // update position every second so enemy doesnt get stuck
            if (m_knowledgeBase.playerTransform)
            {
                if (m_cooldown >= 0.5f)
                {
                    m_cooldown = 0.0f;
                    m_navMeshAgent.SetDestination(m_knowledgeBase.playerTransform.position);
                }
            }
            //else
            //{
            //    m_motivationActionProperties.canInterrupt = true;
            //}
        }
    }
}