using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Trees.Subtrees
{
    public struct PatrollingSubtree
    {
        public Node m_root;

        private GameObject[] m_patrolPoints;
        private Root m_behaviorTree;
        private NavMeshAgent m_navMeshAgent;
        private int m_currentPatrolPoint;

        public void Create(Root behaviorTree, GameObject[] patrolPoints, NavMeshAgent navMeshAgent)
        {
            m_behaviorTree = behaviorTree;
            m_patrolPoints = patrolPoints;
            m_navMeshAgent = navMeshAgent;
            m_currentPatrolPoint = 0;

            m_root =
                new Repeater(
                    new Sequence
                    (
                        new Wait(3.0f),
                        new Action(SetNextPatrolPoint),
                        new NavMoveTo(m_navMeshAgent, "nextPosition")
                    )
                );
        }

        void SetNextPatrolPoint()
        {
            m_behaviorTree.Blackboard.Set("nextPosition", m_patrolPoints[m_currentPatrolPoint].transform.position);

            if (++m_currentPatrolPoint >= m_patrolPoints.Length)
            {
                m_currentPatrolPoint = 0;
            }
        }
    }
}
