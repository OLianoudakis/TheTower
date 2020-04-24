using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public struct PatrollingTree
    {
        public Node m_root;
        
        private Root m_behaviorTreeRoot;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;

            m_root =
                new Sequence
                (
                    new Action(SetNextPatrolPoint),
                    new BlackboardCondition("nextPosition", Operator.IS_SET, true, Stops.NONE, // only if next position exist
                        new Sequence
                        (
                            new Action(MoveTo),
                            new WaitForCondition(IsOnSpot,
                                new Action(AtPatrolPoint)
                            ),
                            new Wait("waitTimeAtPoints")
                        )
                    )
                );
        }

        void SetNextPatrolPoint()
        {
            Vector3[] patrolPoints = m_behaviorTreeRoot.Blackboard.Get("patrolPoints") as Vector3[];
            int index = (int)m_behaviorTreeRoot.Blackboard.Get("patrolPointsIndex");
            // check bounds
            if ((patrolPoints != null) && (index < patrolPoints.Length))
            {
                m_behaviorTreeRoot.Blackboard.Set("nextPosition", patrolPoints[index]);
            }
            else
            {
                m_behaviorTreeRoot.Blackboard.Unset("nextPosition");
            }
        }

        void AtPatrolPoint()
        {
            int animation = AnimationConstants.AnimButtlerIdle;
            if (m_behaviorTreeRoot.Blackboard.Isset("atPatrolPointAnimation"))
            {
                animation = (int)m_behaviorTreeRoot.Blackboard.Get("atPatrolPointAnimation");
            }
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, animation);

            // set next patrol point
            int index = (int)m_behaviorTreeRoot.Blackboard.Get("patrolPointsIndex");
            if (++index >= (m_behaviorTreeRoot.Blackboard.Get("patrolPoints") as Vector3[]).Length)
            {
                index = 0;
            }
            m_behaviorTreeRoot.Blackboard.Set("patrolPointsIndex", index);
        }

        private void MoveTo()
        {
            int animation = AnimationConstants.AnimButtlerWalk;
            if (m_behaviorTreeRoot.Blackboard.Isset("patrolingAnimation"))
            {
                animation = (int)m_behaviorTreeRoot.Blackboard.Get("patrolingAnimation");
            }
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, animation);
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("nextPosition"));
        }

        private bool IsOnSpot()
        {
            Vector3 sittablePosition = (Vector3)m_behaviorTreeRoot.Blackboard.Get("nextPosition");
            if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
            {
                return true;
            }
            return false;
        }
    }
}
