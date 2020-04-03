using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using AI.Behavior.Trees;
using UnityEngine.AI;
using Environment;

namespace AI.Behavior.MotivationActions.Actions
{
    public class LazyPatrolAction : MonoBehaviour
    {
        [SerializeField]
        PersonalityType m_personalityType;

        [SerializeField]
        private float m_timeBetweenComments = 3.0f;

        [SerializeField]
        private GameObject m_patrolPointsGroup;

        [SerializeField]
        private float m_waitTimeAtPoints = 3.0f;

        [SerializeField]
        private float m_stamina = 50.0f;

        [SerializeField]
        private float m_sittingTime = 10.0f;

        private bool m_actionInitialized = false;
        private bool m_isStaminaEmpty = true;
        private Root m_behaviorTree;

        private void Awake()
        { 
            NavMeshAgent navmesh = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            TextMesh floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
            MotivationActionsCommentsCatalogue catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (    
                new Service(m_stamina, EmptyStamina,
                    new Selector
                    (
                        new BlackboardCondition("isStaminaEmpty", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                            TreeFactory.CreateSitOnChairTree(m_behaviorTree, navmesh, animator, m_sittingTime, textMesh: floatingTextMesh)
                        ),
                        new Service(m_timeBetweenComments, IsCommentAvailable,
                            new Repeater
                            (
                                new Sequence
                                (
                                    new BlackboardCondition("commentAvailable", Operator.IS_EQUAL, true, Stops.NONE,
                                        TreeFactory.CreateMakeCommentTree(m_behaviorTree, catalogue, floatingTextMesh, m_personalityType)
                                    ),
                                    TreeFactory.CreatePatrollingTree(m_behaviorTree, navmesh, animator)
                                )
                            )
                        )
                    )
                )
            );
            Transform[] tempPoints = m_patrolPointsGroup.GetComponentsInChildren<Transform>();
            Transform[] patrolPoints = new Transform[tempPoints.Length - 1];
            for (int i = 1; i < tempPoints.Length; i++)
            {
                patrolPoints[i - 1] = tempPoints[i];
            }
            m_behaviorTree.Blackboard.Set("patrolPoints", patrolPoints);
            m_behaviorTree.Blackboard.Set("sittableObjects", FindObjectsOfType(typeof(Sittable)) as Sittable[]);
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPoints);
            m_behaviorTree.Blackboard.Set("sittingTime", m_sittingTime);
            m_behaviorTree.Blackboard.Set("commentAvailable", true);

            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void EmptyStamina()
        {
            if (m_isStaminaEmpty)
            {
                m_isStaminaEmpty = false;
                m_behaviorTree.Blackboard.Set("isStaminaEmpty", false);
            }
            else
            {
                m_isStaminaEmpty = true;
                m_behaviorTree.Blackboard.Set("isStaminaEmpty", true);
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
                m_behaviorTree.Blackboard.Unset("rotationDifference");
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
