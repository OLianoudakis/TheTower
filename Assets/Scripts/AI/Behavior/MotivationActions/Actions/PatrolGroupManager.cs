using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior.MotivationActions.Actions
{
    public class PatrolGroupManager : MonoBehaviour
    {
        private Vector3[] m_patrolPoints;
        private int m_currentIndex = 0;

        public Vector3[] patrolPoints
        {
            get { return m_patrolPoints; }
        }

        public int index
        {
            get { return m_currentIndex; }
            set { m_currentIndex = value; }
        }

        private void Awake()
        {
            m_patrolPoints = new Vector3[transform.childCount];
            int i = 0;
            foreach (Transform child in transform)
            {
                // skip self
                if (child != transform)
                {
                    m_patrolPoints[i++] = child.position;
                }
            }
        }
    }
}
