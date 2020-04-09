﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using Environment;
using AI.Behavior.Trees.Tasks;

namespace AI.Behavior.Trees
{
    public struct ObserveMovableSubtree
    {
        public Node m_root;

        private Root m_behaviorTreeRoot;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private FloatingTextBehavior m_textMesh;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, FloatingTextBehavior textMesh = null)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_textMesh = textMesh;

            m_root =
                new Sequence
                (
                    new Action(MoveTo),
                    new WaitForCondition(IsOnSpot,
                        new Sequence
                        (
                            new Action(Observe),
                            new Wait("observeMovableObjectsTime")
                        )
                    )
                );
        }

        private bool IsOnSpot()
        {
            Debug.Log("Is on spot");
            Vector3 sittablePosition = (Vector3)m_behaviorTreeRoot.Blackboard.Get("movablePosition");
            Vector3 rotation = m_navMeshAgent.transform.rotation.eulerAngles;
            m_navMeshAgent.transform.LookAt((Vector3)m_behaviorTreeRoot.Blackboard.Get("movableObjectPosition"));
            float rotateAddition = (rotation.y + (m_navMeshAgent.transform.rotation.eulerAngles.y - rotation.y) / 10.0f);
            m_navMeshAgent.transform.rotation = Quaternion.Euler(rotation.x, rotateAddition, rotation.z);
            if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
            {
                rotation = m_navMeshAgent.transform.rotation.eulerAngles;
                m_navMeshAgent.transform.LookAt((Vector3)m_behaviorTreeRoot.Blackboard.Get("movableObjectPosition"));
                m_navMeshAgent.transform.rotation = Quaternion.Euler(rotation.x, m_navMeshAgent.transform.rotation.eulerAngles.y, rotation.z);
                return true;
            }
            return false;
        }

        private void MoveTo()
        {
            Debug.Log("Move To");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("movablePosition"));
        }

        private void Observe()
        {
            //TODO add proper animation here and make it face the object
            Debug.Log("Observe");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerLookAround);
            if (m_textMesh && m_behaviorTreeRoot.Blackboard.Isset("movableName"))
            {
                m_textMesh.ChangeText("Interesting " + (string)m_behaviorTreeRoot.Blackboard.Get("movableName"));
            }
        }
    }
}