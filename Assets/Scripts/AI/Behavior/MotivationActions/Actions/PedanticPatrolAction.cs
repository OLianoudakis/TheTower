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
        private GameObject m_patrolPointsGroup;

        [SerializeField]
        private float m_waitTimeAtPatrolPoints = 3.0f;

        [SerializeField]
        private float m_observeMovableObjectsTime = 3.0f;
        
        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        private Movable m_lastVisited;
        NavMeshAgent m_navMeshAgent;
        Movable[] m_movableObjects;

        private void Awake()
        {
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_movableObjects = FindObjectsOfType(typeof(Movable)) as Movable[];
            FloatingTextBehavior floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(FloatingTextBehavior)) as FloatingTextBehavior;
            MotivationActionsCommentsCatalogue catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Service(0.5f, FindClosestMovableObject,
                    new Selector
                    (
                        new BlackboardCondition("isMovableAvailable", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                            TreeFactory.CreateObserveMovableTree(m_behaviorTree, m_navMeshAgent, animator, floatingTextMesh)
                        ),
                        new Repeater
                        (
                            new Sequence
                            (
                                TreeFactory.CreatePatrollingTree(m_behaviorTree, m_navMeshAgent, animator),
                                TreeFactory.CreateMakeCommentTree(m_behaviorTree, catalogue, floatingTextMesh, m_personalityType)
                            )
                        )
                    )
                )
            );
            Transform[] tempPoints = m_patrolPointsGroup.GetComponentsInChildren<Transform>();
            Transform[] patrolPoints = new Transform[tempPoints.Length - 1];
            for (int i = 1; i < tempPoints.Length; i++)
            {
                patrolPoints[i-1] = tempPoints[i];
            }
            m_behaviorTree.Blackboard.Set("patrolPoints", patrolPoints);
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
            Debug.Log("Finding Movable Object");
            m_behaviorTree.Blackboard.Set("isMovableAvailable", false);

            foreach (Movable movable in m_movableObjects)
            {
                if (m_navMeshAgent && (m_lastVisited != movable) && movable.CanMove(m_navMeshAgent.transform))
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
            if (m_actionInitialized)
            {
                m_navMeshAgent.isStopped = false;
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Stop();
                m_navMeshAgent.isStopped = true;
                m_navMeshAgent.ResetPath();
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
