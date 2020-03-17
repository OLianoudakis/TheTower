using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Actions
{
    public class InvestigateAction : MonoBehaviour
    {
        private NavMeshAgent m_navMeshAgent;
        private Vector3 m_sourceOfSuspicion = Vector3.zero;

        public void ActivateAction()
        {
            //TODO Look around animation
        }

        public void ActivateActon(Vector3 source)
        {
            m_sourceOfSuspicion = source;
            m_navMeshAgent.SetDestination(m_sourceOfSuspicion);
        }

        private void Start()
        {
            m_navMeshAgent = transform.parent.transform.parent.GetComponent<NavMeshAgent>();
        }
    }
}