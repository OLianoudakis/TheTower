using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public struct ChaseSubtree
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
                new Repeater
                (
                    new Sequence
                    (
                        new Action(UpdatePosition),
                        new BlackboardCondition("targetPosition", Operator.IS_SET, Stops.NONE,
                            new Action(Chase)
                        )
                    )
                );
        }

        private void UpdatePosition()
        {
            Transform targetTransform = m_behaviorTreeRoot.Blackboard.Get("targetTransform") as Transform;
            if (targetTransform)
            {
                Debug.Log("Chase");
                if (m_animator.GetInteger(AnimationConstants.ButtlerAnimationState) != AnimationConstants.AnimButtlerWalk)
                {
                    m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
                }
                m_behaviorTreeRoot.Blackboard.Set("targetPosition", targetTransform.position);
            }
            else
            {
                Debug.Log("Chase stopped");
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
                m_behaviorTreeRoot.Blackboard.Unset("targetPosition");
            }
        }

        private void Chase()
        {
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("targetPosition"));
        }
    }
}
