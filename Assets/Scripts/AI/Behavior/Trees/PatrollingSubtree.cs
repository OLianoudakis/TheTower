using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public struct PatrollingTree
    {
        public Node m_root;

        private Transform[] m_patrolPoints;
        private Root m_behaviorTreeRoot;
        private NavMeshAgent m_navMeshAgent;
        private int m_currentPatrolPoint;
        private Animator m_animator;
        private float m_waitTimeAtPoints;

        public void Create(Root behaviorTreeRoot, GameObject m_patrolPointsGroup, NavMeshAgent navMeshAgent, Animator animator, float waitTimeAtPoints)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_currentPatrolPoint = 0;

            Transform[] tempPoints = m_patrolPointsGroup.GetComponentsInChildren<Transform>();
            m_patrolPoints = new Transform[tempPoints.Length - 1];
            for (int i = 1; i < tempPoints.Length; i++)
            {
                m_patrolPoints.SetValue(tempPoints[i], i - 1);
            }
            m_waitTimeAtPoints = waitTimeAtPoints;

            m_root =
                new Repeater
                (
                    new Sequence
                    (
                        new Wait(m_waitTimeAtPoints),
                        new Action(SetNextPatrolPoint),
                        new NavMoveTo(m_navMeshAgent, "nextPosition")
                    )
                );
        }

        void SetNextPatrolPoint()
        {
            m_behaviorTreeRoot.Blackboard.Set("nextPosition", m_patrolPoints[m_currentPatrolPoint].position);

            if (++m_currentPatrolPoint >= m_patrolPoints.Length)
            {
                m_currentPatrolPoint = 0;
            }
        }
    }
}
