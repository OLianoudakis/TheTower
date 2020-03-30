using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using AI.KnowledgeBase;

namespace AI.Behavior.Trees
{
    public struct RaiseAlarmSubtree
    {
        public Node m_root;

        private Root m_behaviorTreeRoot;
        private Animator m_animator;
        private float m_shoutingTime;

        public void Create(Root behaviorTreeRoot, Animator animator, float shoutingTime)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_animator = animator;
            m_shoutingTime = shoutingTime;

            m_root =
                new BlackboardCondition("alarmAnimationStarted", Operator.IS_NOT_SET, true, Stops.NONE,
                    new Sequence
                    (
                        new Action(StartShouting),
                        new Wait(m_shoutingTime),
                        new Action(StopShouting)
                    )
                );
        }

        private void StartShouting()
        {
            Debug.Log("Shouting");
            m_behaviorTreeRoot.Blackboard.Set("alarmAnimationStarted", true);
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerYell);
        }

        private void StopShouting()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
        }
    }
}
