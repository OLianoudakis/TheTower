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
        private float m_timeBetweenComments = 3.0f;

        [SerializeField]
        private GameObject m_patrolPointsGroup;

        [SerializeField]
        private float m_waitTimeAtPatrolPoints = 3.0f;

        [SerializeField]
        private float m_observeMovableObjectsTime = 3.0f;

        private TextMesh m_floatingTextMesh;
        
        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        private Movable m_lastVisited;
        NavMeshAgent m_navmesh;
        Movable[] m_movableObjects;

        private void Awake()
        {
            m_navmesh = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_movableObjects = FindObjectsOfType(typeof(Movable)) as Movable[];
            TextMesh floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
            MotivationActionsCommentsCatalogue catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Service(0.5f, FindClosestMovableObject,
                    new Selector
                    (
                        new BlackboardCondition("isMovableAvailable", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                            TreeFactory.CreateObserveMovableTree(m_behaviorTree, m_navmesh, animator, floatingTextMesh)
                        ),
                        new Service(m_timeBetweenComments, IsCommentAvailable,
                            new Selector
                            (
                                new BlackboardCondition("commentAvailable", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                                    TreeFactory.CreateMakeCommentTree(m_behaviorTree, catalogue, floatingTextMesh, m_personalityType)
                                ),
                                TreeFactory.CreatePatrollingTree(m_behaviorTree, m_navmesh, animator)
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
            m_behaviorTree.Blackboard.Set("commentAvailable", true);

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
                if (m_navmesh && (m_lastVisited != movable) && movable.CanMove(m_navmesh.transform))
                {
                    m_lastVisited = movable;
                    m_behaviorTree.Blackboard.Set("isMovableAvailable", true);
                    m_behaviorTree.Blackboard.Set("movablePosition", movable.movablePosition.position);
                    if (!movable.name.Equals(""))
                    {
                        m_behaviorTree.Blackboard.Set("movableName", movable.name);
                    }
                    break;
                }
            }
        }

        private void IsCommentAvailable()
        {
            if ((bool)m_behaviorTree.Blackboard.Get("commentAvailable"))
            {
                m_behaviorTree.Blackboard.Set("commentAvailable", false);
            }
            else
            {
                m_behaviorTree.Blackboard.Set("commentAvailable", true);
            }
        }

        private void OnEnable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Stop();
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
