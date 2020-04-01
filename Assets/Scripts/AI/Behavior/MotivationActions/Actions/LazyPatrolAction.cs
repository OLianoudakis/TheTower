﻿using System.Collections;
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

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (    
                new Service(m_stamina, EmptyStamina,
                    new Selector
                    (
                        new BlackboardCondition("isStaminaEmpty", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                            TreeFactory.CreateSitOnChairTree(m_behaviorTree, navmesh, animator, m_sittingTime)
                        ),
                        TreeFactory.CreatePatrollingTree(m_behaviorTree, m_patrolPointsGroup, navmesh, animator)
                    )
                )
            );
            m_behaviorTree.Blackboard.Set("sittableObjects", FindObjectsOfType(typeof(Sittable)) as Sittable[]);
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPoints);
            m_behaviorTree.Blackboard.Set("sittingTime", m_sittingTime);

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
