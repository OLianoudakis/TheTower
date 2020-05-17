using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using AI.Behavior.Trees;
using UnityEngine.AI;
using Environment;
using AI.Behavior.EmotionalActions;

namespace AI.Behavior.MotivationActions.Actions
{
    public class PedanticPatrolAction : MonoBehaviour
    {
        [SerializeField]
        PersonalityType m_personalityType;

        [SerializeField]
        private PatrolGroupManager m_patrolGroupManager;

        [SerializeField]
        private float m_waitTimeAtPatrolPoints = 3.0f;

        [SerializeField]
        private float m_observeMovableObjectsTime = 3.0f;
        
        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        private Movable m_lastVisited;
        private NavMeshAgent m_navMeshAgent;
        private Movable[] m_movableObjects;
        private Animator m_animator;

        private void Awake()
        {
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_movableObjects = FindObjectsOfType(typeof(Movable)) as Movable[];
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            Create();
        }

        private void Create()
        {
            FloatingTextBehavior floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(FloatingTextBehavior)) as FloatingTextBehavior;
            MotivationActionsCommentsCatalogue catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;
            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Service(0.5f, FindClosestMovableObject,
                    new Selector
                    (
                        new BlackboardCondition("isMovableAvailable", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                            TreeFactory.CreateObserveMovableTree(m_behaviorTree, m_navMeshAgent, m_animator, floatingTextMesh)
                        ),
                        new Repeater
                        (
                            new Sequence
                            (
                                TreeFactory.CreatePatrollingTree(m_behaviorTree, m_navMeshAgent, m_animator),
                                TreeFactory.CreateMakeCommentTree(m_behaviorTree, catalogue, floatingTextMesh, m_personalityType)
                            )
                        )
                    )
                )
            );
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPatrolPoints);
            m_behaviorTree.Blackboard.Set("observeMovableObjectsTime", m_observeMovableObjectsTime);

            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void FindClosestMovableObject()
        {
            m_behaviorTree.Blackboard.Set("isMovableAvailable", false);

            foreach (Movable movable in m_movableObjects)
            {
                if (m_navMeshAgent && (m_lastVisited != movable) && movable.CanMove(m_navMeshAgent.transform) && movable.movablePosition)
                {
                    m_lastVisited = movable;
                    m_behaviorTree.Blackboard.Set("isMovableAvailable", true);
                    m_behaviorTree.Blackboard.Set("movablePosition", movable.movablePosition.position);
                    m_behaviorTree.Blackboard.Set("movableObjectPosition", movable.transform.position);
                    if (!movable.name.Equals(""))
                    {
                        m_behaviorTree.Blackboard.Set("movableName", movable.name);
                    }
                    break;
                }
            }
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
