using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Behavior.Actions
{
    public class PatrolAction : MonoBehaviour
    {
        [SerializeField]
        private float m_distanceOffset = 0.1f;
        [SerializeField]
        private float m_waitTimeAtPoints = 3.0f; //in seconds
        [SerializeField]
        private GameObject m_patrolPointsGroup;
        private Transform[] m_patrolPoints;
        private int m_patrolPointsIndex = -1;

        private NavMeshAgent m_navMeshAgent;
        private Vector3 m_currentControlPoint;
        private Animator m_animator;
        private bool m_isPatroling = true;       

        public bool isPatroling
        {
            get { return m_isPatroling; }
            set { m_isPatroling = value; }
        }

        private void Awake()
        {
            m_animator = transform.parent.parent.GetComponentInChildren<Animator>();
            m_navMeshAgent = transform.parent.parent.GetComponent<NavMeshAgent>();
            Transform[] tempPoints = m_patrolPointsGroup.GetComponentsInChildren<Transform>();
            m_patrolPoints = new Transform[tempPoints.Length - 1];
            for (int i = 1; i < tempPoints.Length; i++)
            {
                m_patrolPoints.SetValue(tempPoints[i], i - 1);
            }
        }

        private void Update()
        {
            if (m_navMeshAgent.velocity.magnitude <= 0.0f)
            {
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            }
            else
            {
                m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            }
        }

        private void OnEnable()
        {
            StopCoroutine(Patrol());
            StartCoroutine(Patrol());
        }

        private void NextDestination()
        {
            m_patrolPointsIndex++;
            if (m_patrolPointsIndex >= m_patrolPoints.Length)
            {
                m_patrolPointsIndex = 0;
            }
            m_currentControlPoint = m_patrolPoints[m_patrolPointsIndex].transform.position;
            m_navMeshAgent.SetDestination(m_currentControlPoint);
        }

        private IEnumerator Patrol()
        {
            while (m_isPatroling)
            {
                NextDestination();
                while (Vector3.Distance(m_navMeshAgent.gameObject.transform.position, m_currentControlPoint) > m_distanceOffset)
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForSeconds(m_waitTimeAtPoints);                
            }
        }
    }
}