using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using AI.Behavior.Trees;

namespace AI.Behavior.MotivationActions.Actions
{
    public class PatrolAction : MonoBehaviour
    {
        [SerializeField]
        PersonalityType m_personalityType;

        [SerializeField]
        private float m_waitTimeAtPatrolPoints = 3.0f;

        [SerializeField]
        private PatrolGroupManager m_patrolGroupManager;

        private bool m_actionInitialized = false;
        private Root m_behaviorTree;
        private Animator m_animator;
        private NavMeshAgent m_navMeshAgent;

        private void Awake()
        {
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            Create();
        }

        private void Create()
        {
            MotivationActionsCommentsCatalogue catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;
            FloatingTextBehavior floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(FloatingTextBehavior)) as FloatingTextBehavior;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Sequence
                (
                    TreeFactory.CreatePatrollingTree(m_behaviorTree, m_navMeshAgent, m_animator),
                    TreeFactory.CreateMakeCommentTree(m_behaviorTree, catalogue, floatingTextMesh, m_personalityType)
                )
            );
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPatrolPoints);
            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void OnEnable()
        {
            if (m_behaviorTree.IsStopRequested)
            {
                Create();
            }
            if (m_actionInitialized && !m_behaviorTree.IsActive)
            {
                m_behaviorTree.Blackboard.Set("patrolPoints", m_patrolGroupManager.patrolPoints);
                m_behaviorTree.Blackboard.Set("patrolPointsIndex", m_patrolGroupManager.index);
                m_navMeshAgent.isStopped = false;
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized && m_behaviorTree.IsActive)
            {
                m_behaviorTree.Stop();
                m_navMeshAgent.isStopped = true;
                m_navMeshAgent.ResetPath();
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
                m_patrolGroupManager.index = (int)m_behaviorTree.Blackboard.Get("patrolPointsIndex");
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}