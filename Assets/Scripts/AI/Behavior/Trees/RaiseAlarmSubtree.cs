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
        private FloatingTextBehavior m_textMesh;

        public void Create(Root behaviorTreeRoot, Animator animator, FloatingTextBehavior textMesh = null)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_animator = animator;
            m_textMesh = textMesh;

            m_root =
                new Sequence
                    (
                        new Action(StartShouting),
                        new Wait("shoutingTime"),
                        new Action(StopShouting)
                    );
        }

        private void StartShouting()
        {
            Debug.Log("Shouting");
            if(m_textMesh)
            {
                m_textMesh.ChangeText(m_textMesh.text + " Get Him!.");
            }
            
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerYell);
        }

        private void StopShouting()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
        }
    }
}
