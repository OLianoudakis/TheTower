using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using AI.Behavior.Trees;
using UnityEngine.AI;
using AI.KnowledgeBase;
using Environment;

namespace AI.Behavior.MotivationActions.Actions
{
    public class LazyRaiseAlarmAndChase : MonoBehaviour
    {
        [SerializeField]
        private float m_shoutingTime = 2.0f;

        [SerializeField]
        private float m_playerForgetTimeWhileSeated = 10.0f;

        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        private ShareKnowledge m_shareKnowledge;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private NavMeshAgent m_navMeshAgent;
        private Sittable[] m_sittableObjects;
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
            m_sittableObjects = FindObjectsOfType(typeof(Sittable)) as Sittable[];
            Create();
        }

        private void Create()
        {
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            FloatingTextBehavior floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(FloatingTextBehavior)) as FloatingTextBehavior;
            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Sequence
                (
                    TreeFactory.CreateRaiseAlarmTree(m_behaviorTree, animator, floatingTextMesh),
                    new Selector
                    (
                        new Sequence
                        (
                            new Action(FindClosestSittable),
                            new BlackboardCondition("isSittableAvailable", Operator.IS_EQUAL, true, Stops.NONE,
                                TreeFactory.CreateSitOnChairTree(m_behaviorTree, m_navMeshAgent, animator, 0.0f, false)
                            )
                        ),
                        TreeFactory.CreateChaseTree(m_behaviorTree, m_navMeshAgent, animator)
                    )
                )
            );
            m_behaviorTree.Blackboard.Set("sittingTime", 0.0f);
            m_behaviorTree.Blackboard.Set("shoutingTime", m_shoutingTime);
            m_playerDefaultStopFollowTime = m_knowledgeBase.GetPlayerStopFollowTime();

            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void FindClosestSittable()
        {
            Debug.Log("Finding Chair");
            m_behaviorTree.Blackboard.Set("isSittableAvailable", false);
            foreach (Sittable sittable in m_sittableObjects)
            {
                if (m_navMeshAgent && sittable.CanSit(m_navMeshAgent.transform))
                {
                    m_behaviorTree.Blackboard.Set("isSittableAvailable", true);
                    m_behaviorTree.Blackboard.Set("sittableForwardVector", sittable.sittablePosition.forward);
                    m_behaviorTree.Blackboard.Set("sittablePosition", sittable.sittablePosition.position);
                    m_behaviorTree.Blackboard.Set("sittableName", sittable.name);
                    m_knowledgeBase.SetPlayerStopFollowTime(m_playerForgetTimeWhileSeated);
                    break;
                }
            }
        }

        private void Update()
        {
            if (!m_knowledgeBase.playerTransform)
            {
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
                m_behaviorTree.Blackboard.Set("targetTransform", m_knowledgeBase.playerTransform);
                m_shareKnowledge.Enable();
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
                m_shareKnowledge.Disable();
                m_knowledgeBase.SetPlayerStopFollowTime(m_playerDefaultStopFollowTime);
                m_behaviorTree.Blackboard.Unset("rotationDifference");
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
