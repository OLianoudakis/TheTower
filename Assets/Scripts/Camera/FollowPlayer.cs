using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class FollowPlayer : MonoBehaviour
    {
        [SerializeField]
        private Transform m_playerPosition;
        [SerializeField]
        private Vector3 m_offset = new Vector3(-3.0f, 7.0f, -3.0f);

        // Update is called once per frame
        void Update()
        {
            transform.position = m_playerPosition.position + m_offset;
        }
    }
}
