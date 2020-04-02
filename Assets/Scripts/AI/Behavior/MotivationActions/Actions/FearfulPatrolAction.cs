using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using AI.Behavior.Trees;
using UnityEngine.AI;
using AI.Behavior.EmotionalActions;
using Environment;

namespace AI.Behavior.MotivationActions.Actions
{
    public class FearfulPatrolAction : MonoBehaviour
    {
        [SerializeField]
        PatrolCommentType m_PatrolCommentType;

        [SerializeField]
        private GameObject m_patrolPointsGroup;

        [SerializeField]
        private float m_waitTimeAtPoints = 0.1f;

        private PatrolCommentsCatalogue m_patrolCatalogueReference;
        private TextMesh m_floatingTextMesh;
        private float m_chanceToComment = 85.0f;

        private bool m_actionInitialized = false;
        private bool m_isStaminaEmpty = true;
        private Root m_behaviorTree;

        private void Awake()
        {
            NavMeshAgent navmesh = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            Animator animator = transform.parent.parent.GetComponentInChildren(typeof(Animator)) as Animator;
            m_floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(TextMesh)) as TextMesh;
            m_patrolCatalogueReference = FindObjectOfType(typeof(PatrolCommentsCatalogue)) as PatrolCommentsCatalogue;

            m_behaviorTree = new Root();
            m_behaviorTree.Create
            (
                new Selector
                (                    
                    TreeFactory.CreatePatrollingTree(m_behaviorTree, m_patrolPointsGroup, navmesh, animator)
                )
            );
            m_behaviorTree.Blackboard.Set("waitTimeAtPoints", m_waitTimeAtPoints);

            // attach debugger to see what's going on in the inspector
#if UNITY_EDITOR
            Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
            debugger.BehaviorTree = m_behaviorTree;
#endif
        }

        private void OnEnable()
        {
            StartCoroutine(MakeComment());
            if (m_actionInitialized)
            {
                m_behaviorTree.Start();
            }
        }

        private void OnDisable()
        {
            StopCoroutine(MakeComment());
            if (m_actionInitialized)
            {
                m_behaviorTree.Stop();
                m_behaviorTree.Blackboard.Unset("rotationDifference");
            }
            else
            {
                m_actionInitialized = true;
            }
        }

        private IEnumerator MakeComment()
        {
            float chanceToComment = 0.0f;
            while(true)
            {
                float seconds = UnityEngine.Random.Range(1.0f, 5.0f);
                yield return new WaitForSeconds(seconds);
                float rollCommentChance = UnityEngine.Random.Range(0.0f, chanceToComment);
                if (rollCommentChance > (100.0f - m_chanceToComment))
                {
                    m_floatingTextMesh.text = m_patrolCatalogueReference.GetPatrolComment(m_PatrolCommentType);
                    yield return new WaitForSeconds(3.0f);
                    m_floatingTextMesh.text = " ";
                    chanceToComment = 0.0f;
                }
                else
                {
                    chanceToComment += 10.0f;
                }
            }
        }
    }
}
