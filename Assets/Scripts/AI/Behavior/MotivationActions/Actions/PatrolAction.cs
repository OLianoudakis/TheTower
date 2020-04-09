using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Behavior.MotivationActions.Actions
{
    public class PatrolAction : MonoBehaviour
    {
        [SerializeField]
        PersonalityType m_personalityType;

        [SerializeField]
        private float m_waitTimeAtPoints = 3.0f;

        [SerializeField]
        private GameObject m_patrolPointsGroup;

        private Transform[] m_patrolPoints;
        private int m_patrolPointsIndex = -1;

        private MotivationActionsCommentsCatalogue m_catalogue;
        private NavMeshAgent m_navMeshAgent;
        private Vector3 m_currentControlPoint;
        private Animator m_animator;
        private FloatingTextBehavior m_floatingTextMesh;
        private float m_currentWaitTime = 0.0f;
        private bool m_atPatrolPoint = true;

        private void Awake()
        {
            m_catalogue = FindObjectOfType(typeof(MotivationActionsCommentsCatalogue)) as MotivationActionsCommentsCatalogue;
            m_animator = transform.parent.parent.GetComponentInChildren<Animator>();
            m_navMeshAgent = transform.parent.parent.GetComponent<NavMeshAgent>();
            m_floatingTextMesh = transform.parent.parent.GetComponentInChildren(typeof(FloatingTextBehavior)) as FloatingTextBehavior;

            Transform[] tempPoints = m_patrolPointsGroup.GetComponentsInChildren<Transform>();
            m_patrolPoints = new Transform[tempPoints.Length - 1];
            for (int i = 1; i < tempPoints.Length; i++)
            {
                m_patrolPoints.SetValue(tempPoints[i], i - 1);
            }
        }

        private void Update()
        {
            m_currentWaitTime += Time.deltaTime;

            if (!m_atPatrolPoint && Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(m_currentControlPoint.x, 0.0f, m_currentControlPoint.z)) <= MathConstants.SquaredDistance)
                {
                m_atPatrolPoint = true;
                m_currentWaitTime = 0.0f;
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
                }

            if (m_atPatrolPoint && m_currentWaitTime >= m_waitTimeAtPoints)
            {
                m_atPatrolPoint = false;
                NextDestination();
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
                int rollCommentChance = UnityEngine.Random.Range(0, 2);
                if (rollCommentChance == 1)
                {
                    m_floatingTextMesh.ChangeText(m_catalogue.GetComment(m_personalityType));
                }
            }
        }

        private void OnEnable()
        {
            m_navMeshAgent.isStopped = false;
        }

        private void OnDisable()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            m_navMeshAgent.isStopped = true;
            m_navMeshAgent.ResetPath();
        }

        private void NextDestination()
        {
            ++m_patrolPointsIndex;
            if (m_patrolPointsIndex >= m_patrolPoints.Length)
            {
                m_patrolPointsIndex = 0;
            }
            m_currentControlPoint = m_patrolPoints[m_patrolPointsIndex].transform.position;
            m_navMeshAgent.SetDestination(m_currentControlPoint);
        }
    }
}