using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public class TreeFactory
    {
        public delegate Object[] FindObjects(System.Type type);

        public static Node CreatePatrollingTree(Root behaviorTree, GameObject m_patrolPointsGroup, NavMeshAgent navMeshAgent, Animator animator, float waitTimeAtPoints)
        {
            PatrollingTree patrollingSubtree = new PatrollingTree();
            patrollingSubtree.Create(behaviorTree, m_patrolPointsGroup, navMeshAgent, animator, waitTimeAtPoints);
            return patrollingSubtree.m_root;
        }

        public static Node CreateGoToTree(Root behaviorTree, NavMeshAgent navMeshAgent)
        {
            GoToTree goToSubtree = new GoToTree();
            goToSubtree.Create(behaviorTree, navMeshAgent);
            return goToSubtree.m_root;
        }

        public static Node CreateSitOnChairTree(Root behaviorTree, FindObjects findObjectCallBack, NavMeshAgent navMeshAgent, Animator animator, float sittingTime)
        {
            SitOnChairSubtree sitOnChairSubtree = new SitOnChairSubtree();
            sitOnChairSubtree.Create(behaviorTree, findObjectCallBack, navMeshAgent, animator, sittingTime);
            return sitOnChairSubtree.m_root;
        }
    }
}
