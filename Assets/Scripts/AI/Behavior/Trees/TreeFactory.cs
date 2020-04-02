using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public class TreeFactory
    {
        public static Node CreatePatrollingTree(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator)
        {
            PatrollingTree patrollingSubtree = new PatrollingTree();
            patrollingSubtree.Create(behaviorTreeRoot, navMeshAgent, animator);
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
            sitOnChairSubtree.Create(behaviorTreeRoot, navMeshAgent, animator);
            return sitOnChairSubtree.m_root;
        }

        public static Node CreateRaiseAlarmTree(Root behaviorTreeRoot, Animator animator)
        {
            RaiseAlarmSubtree raiseAlarmSubtree = new RaiseAlarmSubtree();
            raiseAlarmSubtree.Create(behaviorTreeRoot, animator);
            return raiseAlarmSubtree.m_root;
        }

        public static Node CreateObserveMovableTree(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator)
        {
            ObserveMovableSubtree raiseAlarmSubtree = new ObserveMovableSubtree();
            raiseAlarmSubtree.Create(behaviorTreeRoot, navMeshAgent, animator);
            return raiseAlarmSubtree.m_root;
        }

        public static Node CreateRunAwayTree(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, Transform agentTransform)
        {
            RunAwaySubtree runAwaySubtree = new RunAwaySubtree();
            runAwaySubtree.Create(behaviorTreeRoot, navMeshAgent, animator, agentTransform);
            return runAwaySubtree.m_root;
        }
    }
}
