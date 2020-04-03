using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using AI.KnowledgeBase;

namespace AI.Behavior.Trees
{
    public struct RunAwaySubtree
    {
        public Node m_root;

        private Root m_behaviorTreeRoot;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private Transform m_agentTransform;
        private Transform m_playerTransform;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, Transform agentTransform)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_agentTransform = agentTransform;

            m_root =
                new Sequence
                (
                    new Action(MoveAwayFromPlayer),
                    new NavMoveTo(m_navMeshAgent, "nextPosition")
                );
        }

        private void MoveAwayFromPlayer()
        {
            Vector3 toPlayer = (m_behaviorTreeRoot.Blackboard.Get("targetTransform") as Transform).position - m_agentTransform.position;
            Debug.Log(toPlayer);
            Vector3 targetPosition = m_agentTransform.position + (toPlayer.normalized * -3.0f);
            Debug.Log(targetPosition);

            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 1.0f, NavMesh.AllAreas))
            {
                m_behaviorTreeRoot.Blackboard.Set("nextPosition", navMeshHit.position);
                return;
            }

            if (NavMesh.FindClosestEdge(targetPosition, out navMeshHit, NavMesh.AllAreas))
            {
                m_behaviorTreeRoot.Blackboard.Set("nextPosition", navMeshHit.position);
                return;
            }

            m_behaviorTreeRoot.Blackboard.Set("nextPosition", targetPosition);
            Debug.Log("Didn't find a suitable position");
        }
    }
}
