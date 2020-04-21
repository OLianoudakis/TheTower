using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using AI.Behavior.Trees;
using UnityEngine.AI;
using AI.KnowledgeBase;
using Environment;
using Player.EmptyClass;

namespace AI.Behavior.MotivationActions.Actions
{
    public class RunAndShoutAction : MonoBehaviour
    {
        [SerializeField]
        private Transform m_targetPosition;

        [SerializeField]
        private float m_playerForgetTime = 15.0f;

        [SerializeField]
        private float m_shoutingTime = 2.0f;

        [SerializeField]
        private float m_runningMoveSpeed = 3.5f;

        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        private ShareKnowledge m_shareKnowledge;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private NavMeshAgent m_navMeshAgent;
        private float m_previousMoveSpeed;
        private float m_playerDefaultStopFollowTime;
        private MotivationActionProperties m_motivationActionProperties;

        public Object[] FindObjects(System.Type type)
        {
            return FindObjectsOfType(type);
        }

        private void Awake()
        {
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_shareKnowledge = transform.parent.parent.GetComponentInChildren(typeof(ShareKnowledge)) as ShareKnowledge;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
            m_motivationActionProperties = GetComponent(typeof(MotivationActionProperties)) as MotivationActionProperties;
            Create();
        }

        private void Create()
        {
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Sequence
                (
                    TreeFactory.CreateRaiseAlarmTree(m_behaviorTree, animator),
                    new Repeater
                    (
                        new Sequence
                        (
                            new Action(CheckTargetPosition),
                            TreeFactory.CreateRunAwayTree(m_behaviorTree, m_navMeshAgent, animator, transform.parent.parent)
                        )
                    )
                )
            );
            m_behaviorTree.Blackboard.Set("shoutingTime", m_shoutingTime);
            m_behaviorTree.Blackboard.Set("targetPosition", m_targetPosition.position);
            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void CheckTargetPosition()
        {
            bool isNewPoint = false;
            Vector3 newPoint = Vector3.zero;
            if (m_knowledgeBase.playerTransform)
            {
                isNewPoint = true;
                newPoint = m_knowledgeBase.playerTransform.position;
            }
            else if (m_knowledgeBase.noiseHeard)
            {
                isNewPoint = true;
                newPoint = m_knowledgeBase.GetNoisePosition();
            }
            else if (m_knowledgeBase.playerSuspicion)
            {
                isNewPoint = true;
                newPoint = m_knowledgeBase.GetLastKnownPlayerPosition();
            }
            else if (m_knowledgeBase.playerHiding)
            {
                isNewPoint = true;
                newPoint = m_knowledgeBase.GetLastKnownPlayerPosition();
            }
            object targetPosition = m_behaviorTree.Blackboard.Get("playerPosition");
            if (isNewPoint)
            {
                m_behaviorTree.Blackboard.Set("playerPosition", newPoint);
            }
            else
            {
                m_behaviorTree.Blackboard.Unset("playerPosition");
                m_motivationActionProperties.canInterrupt = true;
            }
        }

        private void OnEnable()
        {
            if (m_behaviorTree.IsStopRequested)
            {
                Create();
            }
            if (m_actionInitialized && !m_behaviorTree.IsActive)
            {
                m_shareKnowledge.Enable();
                m_navMeshAgent.isStopped = false;
                m_motivationActionProperties.canInterrupt = false;
                m_previousMoveSpeed = m_navMeshAgent.speed;
                m_navMeshAgent.speed = m_runningMoveSpeed;
                m_playerDefaultStopFollowTime = m_knowledgeBase.GetPlayerStopFollowTime();
                m_knowledgeBase.SetPlayerStopFollowTime(m_playerForgetTime);
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
                m_navMeshAgent.speed = m_previousMoveSpeed;
                m_shareKnowledge.Disable();
                m_knowledgeBase.SetPlayerStopFollowTime(m_playerDefaultStopFollowTime);
                m_behaviorTree.Blackboard.Unset("playerPosition");
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
