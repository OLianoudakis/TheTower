using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using AI.Trees.Subtrees;

namespace AI.Trees
{
    public class NodeFactory
    {
        public static Node CreatePatrollingSubtree(Root behaviorTree, GameObject[] patrolPoints, NavMeshAgent navMeshAgent)
        {
            PatrollingSubtree patrollingSubtree = new PatrollingSubtree();
            patrollingSubtree.Create(behaviorTree, patrolPoints, navMeshAgent);
            return patrollingSubtree.m_root;
        }

        public static Node CreateGoToSubtree(Root behaviorTree, NavMeshAgent navMeshAgent)
        {
            GoToSubtree goToSubtree = new GoToSubtree();
            goToSubtree.Create(behaviorTree, navMeshAgent);
            return goToSubtree.m_root;
        }
    }
}
