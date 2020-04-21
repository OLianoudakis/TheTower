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
        private bool m_isCrouching;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, Transform agentTransform)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_agentTransform = agentTransform;
            m_isCrouching = false;

            m_root =
                new Sequence
                (
                    new Action(SetMoveAwayPosition),
                    new Action(MoveTo)
                );
        }

        private void SetMoveAwayPosition()
        {
            if (m_behaviorTreeRoot.Blackboard.Isset("targetPosition") && m_behaviorTreeRoot.Blackboard.Isset("playerPosition"))
            {
                Vector3 toPlayer = (Vector3)m_behaviorTreeRoot.Blackboard.Get("playerPosition") - m_agentTransform.position;
                Vector3 targetPosition = Vector3.zero;
                float distanceFromPlayer = toPlayer.magnitude;
                if (distanceFromPlayer <= 2.0f)
                {
                    targetPosition = m_agentTransform.position + (toPlayer.normalized * -2.0f);
                    NavMeshHit navMeshHit;
                    if (NavMesh.SamplePosition(targetPosition, out navMeshHit, 0.1f, NavMesh.AllAreas))
                    {
                        toPlayer = (Vector3)m_behaviorTreeRoot.Blackboard.Get("playerPosition") - navMeshHit.position;
                        distanceFromPlayer = toPlayer.magnitude;
                        if (distanceFromPlayer > 3.0f)
                        {
                            m_behaviorTreeRoot.Blackboard.Set("nextPosition", navMeshHit.position);
                            return;
                        }
                    }
                    if (NavMesh.FindClosestEdge(targetPosition, out navMeshHit, NavMesh.AllAreas))
                    {
                        m_behaviorTreeRoot.Blackboard.Set("nextPosition", navMeshHit.position);
                        return;
                    }
                    m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerCrouch);
                    m_navMeshAgent.isStopped = true;
                    m_behaviorTreeRoot.Blackboard.Unset("nextPosition");
                    Debug.Log("Didn't find a suitable position");
                }
                else
                {
                    m_behaviorTreeRoot.Blackboard.Set("nextPosition", (Vector3)m_behaviorTreeRoot.Blackboard.Get("targetPosition"));
                }
            }
        }

        private void MoveTo()
        {
            Debug.Log("Move To");
            if (m_behaviorTreeRoot.Blackboard.Isset("nextPosition"))
            {
                m_navMeshAgent.isStopped = false;
                int animation = AnimationConstants.AnimButtlerWalk;
                if (m_behaviorTreeRoot.Blackboard.Isset("patrolingAnimation"))
                {
                    animation = (int)m_behaviorTreeRoot.Blackboard.Get("patrolingAnimation");
                }
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, animation);
                m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("nextPosition"));
            }
        }
    }
}
