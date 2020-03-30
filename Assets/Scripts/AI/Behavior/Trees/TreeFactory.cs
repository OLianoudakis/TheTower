using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public class TreeFactory
    {
        public static Node CreatePatrollingTree(Root behaviorTreeRoot, GameObject m_patrolPointsGroup, NavMeshAgent navMeshAgent, Animator animator, float waitTimeAtPoints)
        {
            PatrollingTree patrollingSubtree = new PatrollingTree();
            patrollingSubtree.Create(behaviorTreeRoot, m_patrolPointsGroup, navMeshAgent, animator, waitTimeAtPoints);
            return patrollingSubtree.m_root;
        }

        public static Node CreateChaseTree(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator)
        {
            ChaseSubtree chaseSubtree = new ChaseSubtree();
            chaseSubtree.Create(behaviorTreeRoot, navMeshAgent, animator);
            return chaseSubtree.m_root;
        }

        public static Node CreateSitOnChairTree(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, float sittingTime, bool useStamina = true)
        {
            SitOnChairSubtree sitOnChairSubtree = new SitOnChairSubtree();
            sitOnChairSubtree.Create(behaviorTreeRoot, navMeshAgent, animator, sittingTime);
            return sitOnChairSubtree.m_root;
        }

        public static Node CreateRaiseAlarm(Root behaviorTreeRoot, Animator animator, float shoutingTime)
        {
            RaiseAlarmSubtree raiseAlarmSubtree = new RaiseAlarmSubtree();
            raiseAlarmSubtree.Create(behaviorTreeRoot, animator, shoutingTime);
            return raiseAlarmSubtree.m_root;
        }
    }
}
