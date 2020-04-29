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
        [SerializeField]
        private float m_restoreObjectTime = 2.0f;

        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private NavMeshAgent m_navMeshAgent;
        private Animator m_animator;
        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        private MotivationActionProperties m_motivationActionProperties;

        private void Awake()
        {
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_motivationActionProperties = GetComponent(typeof(MotivationActionProperties)) as MotivationActionProperties;
            Create();
        }

        private void Create()
        {
            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Sequence
                (
                    new Action(LocateMovedObject),
                    new Selector
                    (
                        new BlackboardCondition("movablePosition", Operator.IS_SET, true, Stops.NONE,
                            new Sequence
                            (
                                new Action(MoveTo),
                                new WaitForCondition(IsOnSpot,
                                    new Action(RestoreObjectTransform)
                                ),
                                new Wait(m_restoreObjectTime)
                                //new Action(EnableInterrupt)
                            )
                        ),
                        new WaitUntilStopped(true)
                    )
                )
            );
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void LocateMovedObject()
        {
            if (m_knowledgeBase.environmentObjectMoved)
            {
                Movable movable = m_knowledgeBase.GetMovedObject();
                if (movable && movable.movablePosition)
                {
                    m_behaviorTree.Blackboard.Set("movablePosition", movable.movablePosition.position);
                    m_behaviorTree.Blackboard.Set("movedObjectPosition", movable.transform.position);
                    m_behaviorTree.Blackboard.Set("movedObject", movable);
                }
            }
            else
            {
                m_motivationActionProperties.canInterrupt = true;
            }
        }

        private void MoveTo()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTree.Blackboard.Get("movablePosition"));
        }

        private bool IsOnSpot()
        {
            Vector3 sittablePosition = (Vector3)m_behaviorTree.Blackboard.Get("movablePosition");
            Vector3 rotation = m_navMeshAgent.transform.rotation.eulerAngles;
            m_navMeshAgent.transform.LookAt((Vector3)m_behaviorTree.Blackboard.Get("movedObjectPosition"));
            float rotateAddition = (rotation.y + (m_navMeshAgent.transform.rotation.eulerAngles.y - rotation.y) / 10.0f);
            m_navMeshAgent.transform.rotation = Quaternion.Euler(rotation.x, rotateAddition, rotation.z);
            if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
            {
                rotation = m_navMeshAgent.transform.rotation.eulerAngles;
                m_navMeshAgent.transform.LookAt((Vector3)m_behaviorTree.Blackboard.Get("movedObjectPosition"));
                m_navMeshAgent.transform.rotation = Quaternion.Euler(rotation.x, m_navMeshAgent.transform.rotation.eulerAngles.y, rotation.z);

                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
                return true;
            }
            return false;
        }

        private void RestoreObjectTransform()
        {
            Debug.Log("Restoring objects transform");
            // TODO play animation and wait after animation finishes
            // m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerMoveObject);
            Movable movable = m_behaviorTree.Blackboard.Get("movedObject") as Movable;
            movable.ResetChanges();
            m_knowledgeBase.EnvironmentObjectPositionRestored(movable);
            m_behaviorTree.Blackboard.Unset("movablePosition");
        }

        private void EnableInterrupt()
        {
            m_motivationActionProperties.canInterrupt = true;
        }

        private void OnEnable()
        {
            if (m_behaviorTree.IsStopRequested)
            {
                Create();
            }
            if (m_actionInitialized && !m_behaviorTree.IsActive)
            {
                m_navMeshAgent.isStopped = false;
                m_motivationActionProperties.canInterrupt = false;
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized && m_behaviorTree.IsActive)
            {
                m_behaviorTree.Stop();
                m_navMeshAgent.isStopped = true;
                m_motivationActionProperties.canInterrupt = true;
                m_navMeshAgent.ResetPath();
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
