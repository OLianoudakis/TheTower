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
        private GameObject m_patrolPointsGroup;

        [SerializeField]
        private float m_waitTimeAtPatrolPoints = 3.0f;

        [SerializeField]
        private float m_stamina = 50.0f;

        [SerializeField]
        private float m_sittingTime = 10.0f;

        private float m_staminaCooldown = 0.0f;
        private bool m_actionInitialized = false;
        private bool m_isStaminaEmpty = false;
        private Root m_behaviorTree;
        private Animator m_animator;
        private NavMeshAgent m_navMeshAgent;
        private Sittable[] m_sittableObjects;
        private FloatingTextBehavior m_textMesh;

        private void Awake()
        {
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_sittableObjects = FindObjectsOfType(typeof(Sittable)) as Sittable[];
            m_textMesh = transform.parent.parent.GetComponentInChildren(typeof(FloatingTextBehavior)) as FloatingTextBehavior;
            Create();
        }

        private void Create()
        {
            MotivationActionsCommentsCatalogue catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Service(0.5f, IsSittableAvailable,
                    new Selector
                    (
                        new BlackboardCondition("isSittableAvailable", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                            new Sequence
                            (
                                TreeFactory.CreateSitOnChairTree(m_behaviorTree, m_navMeshAgent, m_animator, m_sittingTime, textMesh: m_textMesh),
                                new Action(FullStamina)
                            )
                        ),
                        new Repeater
                        (
                            new Sequence
                            (
                                TreeFactory.CreatePatrollingTree(m_behaviorTree, m_navMeshAgent, m_animator),
                                TreeFactory.CreateMakeCommentTree(m_behaviorTree, catalogue, m_textMesh, m_personalityType)
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
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPatrolPoints);
            m_behaviorTree.Blackboard.Set("sittingTime", m_sittingTime);
            m_behaviorTree.Blackboard.Set("atPatrolPointAnimation", AnimationConstants.AnimButtlerYawn);
            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void FindClosestSittable()
        {
            // if not set, never sit before
            if (!m_behaviorTree.Blackboard.Isset("isSitting") || ((m_behaviorTree.Blackboard.Isset("isSitting") && !(bool)m_behaviorTree.Blackboard.Get("isSitting"))))
            {
                Debug.Log("Finding Chair");
                m_behaviorTree.Blackboard.Set("isSitting", false);
                m_behaviorTree.Blackboard.Set("isSittableAvailable", false);
                foreach (Sittable sittable in m_sittableObjects)
                {
                    if (m_navMeshAgent && sittable.CanSit(m_navMeshAgent.transform))
                    {
                        if (m_textMesh)
                        {
                            m_textMesh.ChangeText("I need some rest");
                        }
                        m_behaviorTree.Blackboard.Set("isSitting", true);
                        m_behaviorTree.Blackboard.Set("isSittableAvailable", true);
                        m_behaviorTree.Blackboard.Set("sittableForwardVector", sittable.sittablePosition.forward);
                        m_behaviorTree.Blackboard.Set("sittablePosition", sittable.sittablePosition.position);
                        m_behaviorTree.Blackboard.Set("sittableName", sittable.name);
                        break;
                    }
                }
            }
        }

        private void FullStamina()
        {
            m_isStaminaEmpty = false;
            m_staminaCooldown = 0.0f;
        }

        private void IsSittableAvailable()
        {
            if (m_isStaminaEmpty)
            {
                FindClosestSittable();
            }
            else
            {
                m_staminaCooldown += 0.5f;
                if (m_staminaCooldown >= m_stamina)
                {
                    m_isStaminaEmpty = true;
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
                m_behaviorTree.Blackboard.Unset("rotationDifference");
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
