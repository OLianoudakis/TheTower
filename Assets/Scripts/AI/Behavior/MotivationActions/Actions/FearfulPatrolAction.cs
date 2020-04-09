﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using AI.Behavior.Trees;
using UnityEngine.AI;
using AI.Behavior.EmotionalActions;
using Environment;

namespace AI.Behavior.MotivationActions.Actions
{
    public class FearfulPatrolAction : MonoBehaviour
    {
        [SerializeField]
        PersonalityType m_personalityType;

        [SerializeField]
        private float m_timeBetweenComments = 3.0f;

        [SerializeField]
        private float m_maxTimeBetweenComments = 3.0f;

        [SerializeField]
        private GameObject m_patrolPointsGroup;

        [SerializeField]
        private float m_waitTimeAtPoints = 0.1f;
        
        private bool m_actionInitialized = false;
        private bool m_isStaminaEmpty = true;
        private Root m_behaviorTree;
        private NavMeshAgent m_navMeshAgent;

        private void Awake()
        {
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            FloatingTextBehavior floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(FloatingTextBehavior)) as FloatingTextBehavior;
            MotivationActionsCommentsCatalogue catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Service(m_timeBetweenComments, IsCommentAvailable,
                    new Repeater
                    (
                        new Sequence
                        (
                            new BlackboardCondition("commentAvailable", Operator.IS_EQUAL, true, Stops.NONE,
                                TreeFactory.CreateMakeCommentTree(m_behaviorTree, catalogue, floatingTextMesh, m_personalityType, m_maxTimeBetweenComments)
                            ),
                            TreeFactory.CreatePatrollingTree(m_behaviorTree, m_navMeshAgent, animator)
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
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPoints);
            m_behaviorTree.Blackboard.Set("commentAvailable", false);
            m_behaviorTree.Blackboard.Set("patrolingAnimation", AnimationConstants.AnimButtlerFearWalk);
            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
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
                m_behaviorTree.Blackboard.Unset("rotationDifference");
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}