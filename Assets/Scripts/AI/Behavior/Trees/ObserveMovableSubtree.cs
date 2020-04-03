using System.Collections;
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
        private TextMesh m_textMesh;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator, TextMesh textMesh)
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
                            new Wait("observeMovableObjectsTime"),
                            new Action(EndObserve)
                        )
                    )
                );
        }



        private bool IsOnSpot()
        {
            Debug.Log("Is on spot");
            Vector3 sittablePosition = (Vector3)m_behaviorTreeRoot.Blackboard.Get("movablePosition");
            if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
            {
                return true;
            }
            return false;
        }

        private void MoveTo()
        {
            Debug.Log("Move To");
            m_textMesh.text = "";
            m_navMeshAgent.isStopped = false;
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("movablePosition"));
        }

        private void Observe()
        {
            Debug.Log("Observe");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerLookAround);
            if (m_behaviorTreeRoot.Blackboard.Isset("movableName"))
            {
                m_textMesh.text = "What a beautiful " + (string)m_behaviorTreeRoot.Blackboard.Get("movableName");
            }
        }

        private void EndObserve()
        {
            m_textMesh.text = "";
        }
    }
}
