using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.StateHandling
{
    public class TransitionHandler : MonoBehaviour
    {
        private SortedList<int, GameObject> m_availableTransitions = new SortedList<int, GameObject>();

        public void AddActiveTransition(int priority, GameObject state)
        {
            if (!m_availableTransitions.ContainsKey(priority))
            {
                m_availableTransitions.Add(priority, state);
            }
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
