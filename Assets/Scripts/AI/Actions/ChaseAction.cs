using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player.EmptyClass;

namespace AI.Actions
{
    public class ChaseAction : MonoBehaviour
    {
        private Transform m_playerTransform;
        private NavMeshAgent m_navMeshAgent;

        private bool m_isChasing = true;

        public void ActivateAction()
        {
            StartCoroutine(Chase());
        }

        public bool isChasing
        {
            get { return m_isChasing; }
            set { m_isChasing = value; }
        }

        private void Start()
        {
            m_navMeshAgent = transform.parent.transform.parent.GetComponent<NavMeshAgent>();
            m_playerTransform = FindObjectOfType<PlayerTagScript>().gameObject.transform;
            //ActivateAction(); //TODO Remove this line
        }

        private IEnumerator Chase()
        {
            while (m_isChasing)
            {
                m_navMeshAgent.SetDestination(m_playerTransform.position);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}