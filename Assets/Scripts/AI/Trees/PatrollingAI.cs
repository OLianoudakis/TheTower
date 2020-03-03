using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Trees
{
    public class PatrollingAI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_patrolPoints;

        private Root m_behaviorTree;
        private int m_currentPatrolPoint = 0;
        
        private void Start()
        {
            NavMeshAgent navmesh = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            // The Behavior Tree
            m_behaviorTree = new Root
            (
                new Selector
                (
                    // patrol around
                    new Repeater(
                        new Sequence
                        (
                            new Wait(3.0f),
                            new Action(SetNextPatrolPoint),
                            new NavMoveTo(navmesh, "nextPosition")
                        )
                    )
                )
            );

            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
            // start the AI
            m_behaviorTree.Start();
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
