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

        private Root m_behaviorTree;
        private NavMeshAgent m_navMeshAgent;
        private Sittable[] m_sittableObjects;
        private Animator m_animator;
        private float m_sittingTime;

        public void Create(Root behaviorTree, TreeFactory.FindObjects findObjectCallBack, NavMeshAgent navMeshAgent, Animator animator, float sittingTime)
        {
            m_sittableObjects = findObjectCallBack(typeof(Sittable)) as Sittable[];
            m_behaviorTree = behaviorTree;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_sittingTime = sittingTime;

            m_root =
                new BlackboardCondition("isStaminaEmpty", Operator.IS_EQUAL, true, Stops.LOWER_PRIORITY_IMMEDIATE_RESTART,
                    new Sequence
                    (
                        new Action(FindClosestChair),
                        new BlackboardCondition("isSittableAvailable", Operator.IS_EQUAL, true, Stops.NONE,
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
                                                new Wait(m_sittingTime),
                                                new Action(StandUp)
                                            )
                                        ),
                                        new Repeater
                                        (
                                            new Action(Rotate)
                                        )
                                    )
                                )
                                
                            )
                        )
                    )
                );
        }

        private void FindClosestChair()
        {
            Debug.Log("Finding Chair");
            bool isThereASittableObject = false;
            foreach (Sittable sittable in m_sittableObjects)
            {
                if (m_navMeshAgent && sittable.CanSit(m_navMeshAgent.transform))
                {
                    isThereASittableObject = true;
                    m_behaviorTree.Blackboard.Set("isSittableAvailable", true);
                    m_behaviorTree.Blackboard.Set("sittableTransformRotationY", sittable.sittablePosition.rotation.y);
                    m_behaviorTree.Blackboard.Set("sittablePosition", sittable.sittablePosition.position);
                    break;
                }
            }
        }

        private void MoveTo()
        {
            Debug.Log("MoveTo");
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTree.Blackboard.Get("sittablePosition"));
        }

        private bool IsOnSpot()
        {
            Vector3 sittablePosition = (Vector3)m_behaviorTree.Blackboard.Get("sittablePosition");
            if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
            {
                return true;
            }
            return false;
        }

        private void Rotate()
        {
            Debug.Log("Rotating");
            m_navMeshAgent.transform.Rotate(0.0f, Time.deltaTime * 100.0f, 0.0f);
            float sittableTransformRotationY = (float)m_behaviorTree.Blackboard.Get("sittableTransformRotationY");
            m_behaviorTree.Blackboard.Set("rotationDifference", sittableTransformRotationY - m_navMeshAgent.transform.rotation.y);
        }

        private void Sit()
        {
            Debug.Log("Sit");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerSit);
        }

        private void StandUp()
        {
            Debug.Log("Stand");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerStand);
            m_behaviorTree.Blackboard.Set("isSittableAvailable", false);
            m_behaviorTree.Blackboard.Unset("rotationDifference");
        }
    }
}
