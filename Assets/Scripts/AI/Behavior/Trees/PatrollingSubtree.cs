using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using NPBehave;

namespace AI.Behavior.Trees
{
    public struct PatrollingTree
    {
        public Node m_root;

        //private Transform[] m_patrolPoints;
        private Root m_behaviorTreeRoot;
        private NavMeshAgent m_navMeshAgent;
        private int m_currentPatrolPoint;
        private Animator m_animator;

        public void Create(Root behaviorTreeRoot, NavMeshAgent navMeshAgent, Animator animator)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_navMeshAgent = navMeshAgent;
            m_animator = animator;
            m_currentPatrolPoint = 0;

            //Transform[] tempPoints = m_patrolPointsGroup.GetComponentsInChildren<Transform>();
            //m_patrolPoints = new Transform[tempPoints.Length - 1];
            //for (int i = 1; i < tempPoints.Length; i++)
            //{
            //    m_patrolPoints.SetValue(tempPoints[i], i - 1);
            //}

            m_root =
                //new Repeater
                //(
                    new Sequence
                    (
                        new Action(SetNextPatrolPoint),
                        new Action(MoveTo),
                        new WaitForCondition(IsOnSpot,
                            new Action(AtPatrolPoint)
                        ),
                        new Wait("waitTimeAtPoints")
                    );
                //);
        }

        void SetNextPatrolPoint()
        {
            if (!m_behaviorTreeRoot.Blackboard.Isset("nextPosition"))
            {
                object patrolPointsObj = m_behaviorTreeRoot.Blackboard.Get("patrolPoints");
                if (patrolPointsObj.GetType() == typeof(Transform[]))
                {
                    Transform[] patrolPoints = patrolPointsObj as Transform[];
                    m_behaviorTreeRoot.Blackboard.Set("nextPosition", patrolPoints[m_currentPatrolPoint].position);
                    if (++m_currentPatrolPoint >= patrolPoints.Length)
                    {
                        m_currentPatrolPoint = 0;
                    }
                }
                else if (patrolPointsObj.GetType() == typeof(Vector3[]))
                {
                    Vector3[] patrolPoints = patrolPointsObj as Vector3[];
                    m_behaviorTreeRoot.Blackboard.Set("nextPosition", patrolPoints[m_currentPatrolPoint]);
                    if (++m_currentPatrolPoint >= patrolPoints.Length)
                    {
                        m_currentPatrolPoint = 0;
                    }
                }
            }
        }

        void AtPatrolPoint()
        {
            int animation = AnimationConstants.AnimButtlerIdle;
            if (m_behaviorTreeRoot.Blackboard.Isset("atPatrolPointAnimation"))
            {
                animation = (int)m_behaviorTreeRoot.Blackboard.Get("atPatrolPointAnimation");
            }
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, animation);
        }

        private void MoveTo()
        {
            Debug.Log("Move To");
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerWalk);
            m_navMeshAgent.isStopped = false;
            m_navMeshAgent.SetDestination((Vector3)m_behaviorTreeRoot.Blackboard.Get("nextPosition"));
        }

        private bool IsOnSpot()
        {
            Debug.Log("Is on spot");
            Vector3 sittablePosition = (Vector3)m_behaviorTreeRoot.Blackboard.Get("nextPosition");
            if (Vector3.SqrMagnitude(new Vector3(m_navMeshAgent.transform.position.x, 0.0f, m_navMeshAgent.transform.position.z)
                - new Vector3(sittablePosition.x, 0.0f, sittablePosition.z)) < MathConstants.SquaredDistance)
            {
                m_behaviorTreeRoot.Blackboard.Unset("nextPosition");
                return true;
            }
            return false;
        }
    }
}
