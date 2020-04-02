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

        private void Awake()
        {
            Animator animator = transform.parent.transform.parent.GetComponentInChildren<Animator>();
            NavMeshAgent navmesh = transform.parent.transform.parent.GetComponent<NavMeshAgent>();

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Sequence
                (
                    new BlackboardCondition("newPointOfInterest", Operator.IS_EQUAL, true, Stops.NONE,
                        new CalculateInvestigationPoints("pointOfInterest", "patrolPoints", 3)
                    ),
                    TreeFactory.CreatePatrollingTree(m_behaviorTree, navmesh, animator)
                )
            );
            m_behaviorTree.Blackboard.Set("atPatrolPointAnimation", AnimationConstants.AnimButtlerLookAround);
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPoints);
        }

        private bool CheckNewPointOfInterest(ref Vector3 pointOfInterest)
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
                newPoint = m_knowledgeBase.GetPlayerSuspicionPosition();
            }
            else if (m_knowledgeBase.playerHiding)
            {
                isNewPoint = true;
                newPoint = m_knowledgeBase.GetLastKnownPlayerPosition();
            }
            if (isNewPoint && (pointOfInterest != newPoint))
            {
                pointOfInterest = newPoint;
                return true;
            }
            return false;
        }

        private void Update()
        {
            Vector3 position = Vector3.zero;
            bool newPosition = CheckNewPointOfInterest(ref position);
            m_behaviorTree.Blackboard.Set("newPointOfInterest", newPosition);
            m_behaviorTree.Blackboard.Set("pointOfInterest", position);
        }

        private void OnEnable()
        {
            if (m_actionInitialized)
            {
                Vector3 position = Vector3.zero;
                if (CheckNewPointOfInterest(ref position))
                {
                    m_behaviorTree.Blackboard.Set("newPointOfInterest", true);
                    m_behaviorTree.Blackboard.Set("pointOfInterest", position);
                    m_behaviorTree.Start();
                }
            }
        }

        private void OnDisable()
        {
            if (m_actionInitialized)
            {
                m_behaviorTree.Blackboard.Set("newPointOfInterest", false);
                m_behaviorTree.Stop();
            }
            else
            {
                m_actionInitialized = true;
            }
        }
    }
}