using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;
using AI.Behavior.Trees;
using AI.Behavior.Trees.Tasks;

namespace AI.Behavior.MotivationActions.Actions
{
    public class InvestigateAction : MonoBehaviour
    {
        [SerializeField]
        private float m_waitTimeAtPoints = 3.0f;

        private Root m_behaviorTree;
        private bool m_actionInitialized = false;
        KnowledgeBase.KnowledgeBase m_knowledgeBase;
        NavMeshAgent m_navMeshAgent;

        private void Awake()
        {
            m_navMeshAgent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
            Create();
        }

        private void Create()
        {
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Selector
                (
                    new Sequence
                    (
                        new Action(CheckNewPointOfInterest),
                        new BlackboardCondition("newPointOfInterest", Operator.IS_EQUAL, true, Stops.NONE,
                            new CalculateInvestigationPoints("pointOfInterest", "patrolPoints", 3)
                        )
                    ),
                    TreeFactory.CreatePatrollingTree(m_behaviorTree, m_navMeshAgent, animator)
                )
            );
            m_behaviorTree.Blackboard.Set("atPatrolPointAnimation", AnimationConstants.AnimButtlerLookAround);
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPoints);

#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void CheckNewPointOfInterest()
        {
            bool isNewPoint = false;
            Vector3 newPoint = Vector3.zero;
            if (m_knowledgeBase.noiseHeard)
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
            object pointOfInterest = m_behaviorTree.Blackboard.Get("pointOfInterest");
            if (isNewPoint && ((pointOfInterest == null) || (Vector3)pointOfInterest != newPoint))
            {
                m_behaviorTree.Blackboard.Set("pointOfInterest", newPoint);
                m_behaviorTree.Blackboard.Set("newPointOfInterest", true);
                return;
            }
            m_behaviorTree.Blackboard.Set("newPointOfInterest", false);
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
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized && m_behaviorTree.IsActive)
            {
                m_behaviorTree.Blackboard.Set("newPointOfInterest", false);
                m_behaviorTree.Stop();
                m_navMeshAgent.isStopped = true;
                m_navMeshAgent.ResetPath();
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}