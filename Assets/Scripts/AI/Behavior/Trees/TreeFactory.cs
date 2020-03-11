using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public class TreeFactory
    {
        public static Node CreatePatrollingTree(Root behaviorTree, GameObject[] patrolPoints, NavMeshAgent navMeshAgent)
        {
            PatrollingTree patrollingSubtree = new PatrollingTree();
            patrollingSubtree.Create(behaviorTree, patrolPoints, navMeshAgent);
            return patrollingSubtree.m_root;
        }

        public static Node CreateGoToTree(Root behaviorTree, NavMeshAgent navMeshAgent)
        {
            GoToTree goToSubtree = new GoToTree();
            goToSubtree.Create(behaviorTree, navMeshAgent);
            return goToSubtree.m_root;
        }
    }
}
