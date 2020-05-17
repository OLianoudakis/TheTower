using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCamera
{
    public class CCTVPlayerDetector : MonoBehaviour
    {
        [SerializeField]
        private Transform m_CCTVPosition;

        [SerializeField]
        private Transform m_lookAt;

        [SerializeField]
        private bool m_followPlayer = false;

        [SerializeField]
        private bool m_freezeTime = true;

        [SerializeField]
        private Constraints m_constraints = null;

        private CameraPositionController m_mainCamera;
        private uint m_stateId;
        private static uint stateId = 0;

        private void Start()
        {
            m_mainCamera = FindObjectOfType<CameraPositionController>();
            if (m_constraints == null)
            {
                m_constraints = new Constraints();
            }
            m_constraints.m_positionBoundaries = new PositionBoundaries();
            BoxCollider boxCollider = GetComponent(typeof(BoxCollider)) as BoxCollider;
            m_constraints.m_positionBoundaries.bounds = new Vector3(
                Mathf.Abs(transform.position.x + boxCollider.center.x - m_CCTVPosition.position.x),
                Mathf.Abs(transform.position.y + boxCollider.center.y - m_CCTVPosition.position.y),
                Mathf.Abs(transform.position.z + boxCollider.center.z - m_CCTVPosition.position.z)
                );
            m_constraints.m_positionBoundaries.triggerCenter = transform.position + boxCollider.center;
            m_stateId = GenerateStateId();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_constraints.m_positionBoundaries.canMove = false;
                m_mainCamera.SetPosition(m_CCTVPosition.position, m_stateId, followPlayer: m_followPlayer, constraints: m_constraints, lookAt: m_lookAt, freezeTime: m_freezeTime);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_constraints.m_positionBoundaries.canMove = true;
            }
        }

        private static uint GenerateStateId()
        {
            return ++stateId;
        }
    }
}
