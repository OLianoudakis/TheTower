﻿using System.Collections;
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
        private float m_sittingTime;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, float sittingTime, bool useStamina = true)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_sittingTime = sittingTime;
            Stops staminaCheck = Stops.LOWER_PRIORITY_IMMEDIATE_RESTART;
            if (!useStamina)
            {
                staminaCheck = Stops.NONE;
            }

            m_root =

                new Sequence
                (
                    new Action(FindClosestChair),
                    new BlackboardCondition("isSittableAvailable", Operator.IS_EQUAL, true, Stops.SELF,
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
                                            new Selector
                                            (
                                                new BlackboardCondition("sittingTime", Operator.IS_EQUAL, 0.0f, Stops.NONE,
                                                    new Repeater
                                                    (
                                                        new Wait(1.0f)
                                                    )
                                                ),
                                                new Wait(m_sittingTime)
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

                        )
                    )
                );
        }

        private void FindClosestChair()
        {
            Debug.Log("Finding Chair");
            m_behaviorTreeRoot.Blackboard.Set("isSittableAvailable", false);
            bool isThereASittableObject = false;

            Sittable[] sittableObjects = m_behaviorTreeRoot.Blackboard.Get("sittableObjects") as Sittable[];
            foreach (Sittable sittable in sittableObjects)
            {
                if (m_navMeshAgent && sittable.CanSit(m_navMeshAgent.transform))
                {
                    Interactible interactible = sittable.GetComponent(typeof(Interactible)) as Interactible;
                    isThereASittableObject = true;
                    m_behaviorTreeRoot.Blackboard.Set("isSittableAvailable", true);
                    m_behaviorTreeRoot.Blackboard.Set("sittableTransformRotationY", interactible.interactiblePosition.rotation.y);
                    m_behaviorTreeRoot.Blackboard.Set("sittablePosition", interactible.interactiblePosition.position);
                    break;
                }
            }
        }

        private void MoveTo()
        {
            Debug.Log("MoveTo");
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("sittablePosition"));
        }

        private bool IsOnSpot()
        {
            Vector3 sittablePosition = (Vector3)m_behaviorTreeRoot.Blackboard.Get("sittablePosition");
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
            float sittableTransformRotationY = (float)m_behaviorTreeRoot.Blackboard.Get("sittableTransformRotationY");
            m_behaviorTreeRoot.Blackboard.Set("rotationDifference", sittableTransformRotationY - m_navMeshAgent.transform.rotation.y);
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
            m_behaviorTreeRoot.Blackboard.Set("isSittableAvailable", false);
            m_behaviorTreeRoot.Blackboard.Unset("rotationDifference");
        }
    }
}