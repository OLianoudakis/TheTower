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

        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        private ShareKnowledge m_shareKnowledge;
        KnowledgeBase.KnowledgeBase m_knowledgeBase;

        public Object[] FindObjects(System.Type type)
        {
            return FindObjectsOfType(type);
        }

        private void Awake()
        {
            NavMeshAgent navmesh = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_shareKnowledge = transform.parent.parent.GetComponentInChildren(typeof(ShareKnowledge)) as ShareKnowledge;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Selector
                (
                    TreeFactory.CreateRaiseAlarm(m_behaviorTree, animator),
                    TreeFactory.CreateSitOnChairTree(m_behaviorTree, navmesh, animator, 0.0f, false),
                    TreeFactory.CreateChaseTree(m_behaviorTree, navmesh, animator)
                )
            );
            m_behaviorTree.Blackboard.Set("sittableObjects", FindObjectsOfType(typeof(Sittable)) as Sittable[]);
            m_behaviorTree.Blackboard.Set("sittingTime", 0.0f);
            m_behaviorTree.Blackboard.Set("shoutingTime", m_shoutingTime);

            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void OnEnable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Blackboard.Set("targetTransform", m_knowledgeBase.playerTransform);
                m_shareKnowledge.enabled = true;
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Stop();
                m_shareKnowledge.enabled = false;
                m_behaviorTree.Blackboard.Unset("rotationDifference");
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}
