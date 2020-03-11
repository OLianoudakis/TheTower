using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using AI.Behavior.Trees;

namespace AI.Behavior
{
    public class PatrollingBehavior : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_patrolPoints;

        private Root m_behaviorTree;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        
        private void Start()
        {
            NavMeshAgent navmesh = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_knowledgeBase = GetComponent(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
            // The Behavior Tree
            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Service(0.5f, UpdateKnowledge,
                    new Selector
                    (
                        TreeFactory.CreateGoToTree(m_behaviorTree, navmesh),
                        TreeFactory.CreatePatrollingTree(m_behaviorTree, m_patrolPoints, navmesh)
                    )
                )
            );

            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
            // start the AI
            m_behaviorTree.Start();


        }

        void UpdateKnowledge()
        {
            if (m_knowledgeBase.playerTransform)
            {
                m_behaviorTree.Blackboard.Set("targetPosition", m_knowledgeBase.playerTransform.position);
            }
        }
    }
}
