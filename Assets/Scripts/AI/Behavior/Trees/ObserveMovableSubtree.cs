using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using Environment;

namespace AI.Behavior.Trees
{
    public struct ObserveMovableSubtree
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
                    new Sequence
                    (
                        new NavMoveTo(m_navMeshAgent, "movablePosition"),
                        //new Action(MoveTo),
                        //new WaitForCondition(IsOnSpot,
                        //new Sequence
                        //(
                        new Action(Observe),
                        new Wait("observeMovableObjectsTime")
                    //)
                    //)
                    )
                );
        }

        

        //private bool IsOnSpot()
        //{
        //    Debug.Log("Is on spot");
        //    Vector3 sittablePosition = (Vector3)m_behaviorTreeRoot.Blackboard.Get("movablePosition");
        //    if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
        //        - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //private void MoveTo()
        //{
        //    Debug.Log("Move To");
        //    m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("movablePosition"));
        //}

        private void Observe()
        {
            Debug.Log("Observe");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerLookAround);
        }
    }
}
