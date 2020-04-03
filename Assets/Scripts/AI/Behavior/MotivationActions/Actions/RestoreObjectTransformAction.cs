using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using UnityEngine.AI;
using Environment;

namespace AI.Behavior.MotivationActions.Actions
{
    public class RestoreObjectTransformAction : MonoBehaviour
    {
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private Root m_behaviorTree;
        private bool m_actionInitialized = false;

        private void Awake()
        {
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Sequence
                (
                    new Action(LocateMovedObject),
                    new BlackboardCondition("movedObjectPosition", Operator.IS_SET, true, Stops.NONE,
                        new Sequence
                        (
                            new NavMoveTo(m_navMeshAgent, "movedObjectPosition"),
                            new Action(RestoreObjectTransform)
                        )
                    )
                )
            );
        }

        private void LocateMovedObject()
        {
            if (m_knowledgeBase.environmentObjectMoved)
            {
                Movable movable = m_knowledgeBase.GetMovedObject();
                if (movable)
                {
                    m_behaviorTree.Blackboard.Set("movedObjectPosition", movable.movablePosition);
                    m_behaviorTree.Blackboard.Set("movedObject", movable);
                }
            }
            else
            {
                m_behaviorTree.Blackboard.Unset("movedObjectPosition");
            }
        }

        private void RestoreObjectTransform()
        {
            Debug.Log("Restoring objects transform");
            // TODO play animation and wait after animation finishes
            // m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerMoveObject);
            Movable movable = m_behaviorTree.Blackboard.Get("movedObject") as Movable;
            movable.ResetChanges();
            m_knowledgeBase.EnvironmentObjectPositionRestored(movable);
            m_behaviorTree.Blackboard.Unset("movedObjectPosition");
        }

        private void OnEnable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Stop();
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
