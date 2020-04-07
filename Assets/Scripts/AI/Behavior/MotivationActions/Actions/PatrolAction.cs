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
        private float m_timeBetweenComments = 3.0f;

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
        private float m_currentCommentTime = 0.0f;
        private float m_chanceToComment;
        private float m_currentChanceToComment;
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
            m_currentCommentTime += Time.deltaTime;

            if (!m_atPatrolPoint 
                && Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
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
            }

            if (m_currentCommentTime >= m_timeBetweenComments)
            {
                float rollCommentChance = UnityEngine.Random.Range(0.0f, m_chanceToComment);
                if (rollCommentChance > (100.0f - m_currentChanceToComment))
                {
                    m_floatingTextMesh.ChangeText(m_catalogue.GetComment(m_personalityType));
                    m_currentChanceToComment = 0.0f;
                    m_currentCommentTime = 0.0f;
                }
                else
                {
                    m_currentChanceToComment += 10.0f;
                }
            }
        }

        private void OnEnable()
        {
            m_navMeshAgent.isStopped = false;
            //m_isPatroling = true;
        }

        private void OnDisable()
        {
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            m_navMeshAgent.isStopped = true;
            m_navMeshAgent.ResetPath();
            //m_isPatroling = false;
            //--m_patrolPointsIndex;
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

        //private IEnumerator Patrol()
        //{
        //    while (m_isPatroling)
        //    {
        //        NextDestination();
        //        while (Vector3.Distance(m_navMeshAgent.gameObject.transform.position, m_currentControlPoint) > m_distanceOffset)
        //        {
        //            yield return new WaitForEndOfFrame();
        //        }
        //        yield return new WaitForSeconds(m_waitTimeAtPoints);                
        //    }
        //}
    }
}