using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public struct GoToTree
    {
        public Node m_root;

        private Root m_behaviorTree;
        private NavMeshAgent m_navMeshAgent;

        public void Create(Root behaviorTree, NavMeshAgent navMeshAgent)
        {
            m_behaviorTree = behaviorTree;
            m_navMeshAgent = navMeshAgent;
            m_root =

                new BlackboardCondition("targetPosition", Operator.IS_SET, Stops.LOWER_PRIORITY,
                    new Repeater
                    (
                        new Sequence
                        (
                            new NavMoveTo(m_navMeshAgent, "targetPosition")
                        )
                    )
                );
        }
    }
}
