using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class SharedAI : MonoBehaviour
    {
        [SerializeField]
        private Transform m_playerTransform;

        public Transform playerTransform
        {
            get { return m_playerTransform; }
        }
    }
}
