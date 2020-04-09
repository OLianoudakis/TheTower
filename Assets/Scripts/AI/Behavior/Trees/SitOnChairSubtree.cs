using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using Environment;

namespace AI.Behavior.Trees
{
    public struct SitOnChairSubtree
    {
        public Node m_root;

        private Root m_behaviorTreeRoot;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private FloatingTextBehavior m_textMesh;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, bool useStamina = true, FloatingTextBehavior textMesh = null)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_textMesh = textMesh;

            Stops staminaCheck = Stops.LOWER_PRIORITY_IMMEDIATE_RESTART;
            if (!useStamina)
            {
                staminaCheck = Stops.NONE;
            }

            m_root =
                new Sequence
                (
                    new Action(MoveTo),
                    new WaitForCondition(IsOnSpot,
                        new Selector
                        (
                            new BlackboardCondition("rotationDifference", Operator.IS_SMALLER_OR_EQUAL, MathConstants.RotationDistance, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                                new Sequence
                                (
                                    new Action(Sit),
                                    new Wait(0.5f),
                                    new Action(IsSitting),
                                    new Selector
                                    (
                                        new BlackboardCondition("sittingTime", Operator.IS_EQUAL, 0.0f, Stops.NONE,
                                            new Repeater
                                            (
                                                new Wait(1.0f)
                                            )
                                        ),
                                        new Wait("sittingTime")
                                    ),
                                    new Action(StandUp)
                                )
                            ),
                            new Repeater
                            (
                                new Action(Rotate)
                            )
                        )
                    )
                );
        }

        private void MoveTo()
        {
            Debug.Log("Move To");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            m_navMeshAgent.isStopped = false;
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("sittablePosition"));
        }

        private bool IsOnSpot()
        {
            Debug.Log("Is on spot");
            Vector3 sittablePosition = (Vector3)m_behaviorTreeRoot.Blackboard.Get("sittablePosition");
            if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
            {
                if (m_textMesh && m_behaviorTreeRoot.Blackboard.Isset("sittableName"))
                {
                    m_textMesh.ChangeText("Let me sit on this fine " + (string)m_behaviorTreeRoot.Blackboard.Get("sittableName"));
                }
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerYawn);
                return true;
            }
            return false;
        }

        private void Rotate()
        {
            Debug.Log("Rotating");
            
            m_navMeshAgent.transform.Rotate(0.0f, Time.deltaTime * 100.0f, 0.0f);
            float sittableTransformRotationY = (float)m_behaviorTreeRoot.Blackboard.Get("sittableTransformRotationY");
            m_behaviorTreeRoot.Blackboard.Set("rotationDifference", sittableTransformRotationY - m_navMeshAgent.transform.eulerAngles.y);
        }

        private void Sit()
        {
            Debug.Log("Sit");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerSit);
        }

        private void IsSitting()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, 999);
            m_behaviorTreeRoot.Blackboard.Set("isSitting", true);
        }

        private void StandUp()
        {
            Debug.Log("Stand");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerStand);
            m_behaviorTreeRoot.Blackboard.Unset("rotationDifference");
            m_behaviorTreeRoot.Blackboard.Set("isSittableAvailable", false);
            m_behaviorTreeRoot.Blackboard.Set("isSitting", false);
        }
    }
}
