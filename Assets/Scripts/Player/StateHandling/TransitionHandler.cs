using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Player.StateHandling
{
    public class TransitionHandler : MonoBehaviour
    {
        private SortedList<int, GameObject> m_availableTransitions = new SortedList<int, GameObject>();
        private NavMeshAgent m_agent;

        public void AddActiveTransition(int priority, GameObject state)
        {
            if (m_agent.enabled == false)
            {
                m_agent.enabled = true;
            }

            if (!m_availableTransitions.ContainsKey(priority))
            {
                m_availableTransitions.Add(priority, state);
            }
        }

        private void Awake()
        {
            m_agent = transform.parent.parent.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
        }

        private void OnEnable()
        {
            m_availableTransitions.Clear();
        }
        private void LateUpdate()
        {
            // just activate first state (having top priority) and disable this state
            foreach (KeyValuePair<int, GameObject> kvp in m_availableTransitions)
            {
                kvp.Value.SetActive(true);
                gameObject.SetActive(false);
                break;
            }
        }
    }
}
