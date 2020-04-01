using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameCamera
{
    [Serializable]
    public class FreezePosition
    {
        public bool x = false;
        public bool y = false;
        public bool z = false;
    }

    [Serializable]
    public class FreezeRotation
    {
        public bool x = false;
        public bool y = false;
        public bool z = false;
    }

    public class PositionBoundaries
    {
        private bool m_canMove = false;
        private Vector3 m_bounds;
        private Vector3 m_triggerCenter;
        public bool canMove
        {
            get { return m_canMove; }
            set { m_canMove = value; }
        }

        public Vector3 bounds
        {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

        public Vector3 triggerCenter
        {
            get { return m_triggerCenter; }
            set { m_triggerCenter = value; }
        }
    }

    [Serializable]
    public class Constraints
    {
        public PositionBoundaries m_positionBoundaries = null;
        public FreezePosition m_freezePosition = null;
        public FreezeRotation m_freezeRotation = null;
    }

    public class CameraPositionController : MonoBehaviour
    {
        public uint m_currentStateId;

        [SerializeField]
        private Transform m_target;

        [SerializeField]
        private float m_lookAtOffset = 0.5f;

        private Transform m_currentLookAt;
        private Vector3 m_currentPosition;
        private Vector3 m_targetLastPosition;
        private bool m_isTranslating = false;
        private bool m_followPlayer = false;
        private Constraints m_constraints;

        public Transform lookAtPosition
        {
            get { return m_currentLookAt; }
            set { m_currentLookAt = value; }
        }

        public float lookAtOffset
        {
            get { return m_lookAtOffset; }
        }

        public Vector3 currentPosition
        {
            get { return m_currentPosition; }
        }

        public void SetPosition(Vector3 targetPosition, float waitTime = 1.5f)
        {
            m_currentPosition = targetPosition;
            StartCoroutine(MoveToPosition(targetPosition, waitTime));
        }

        public void SetPosition(
            Vector3 targetPosition,
            uint newStateId,
            float waitTime = 0.15f,
            bool followPlayer = false,
            Constraints constraints = null,
            Transform lookAt = null
            )
        {
            //if (newStateId != m_currentStateId)
            //{
                m_targetLastPosition = m_target.position;
                m_followPlayer = followPlayer;
                m_constraints = constraints;
                m_currentPosition = targetPosition;
                m_currentStateId = newStateId;
                if (!lookAt)
                {
                    m_currentLookAt = m_target;
                }
                else
                {
                    m_currentLookAt = lookAt;
                }
                StartCoroutine(MoveToPosition(targetPosition, waitTime));
            //}
        }

        public uint cameraStateId
        {
            get { return m_currentStateId; }
        }

        private void Start()
        {
            m_targetLastPosition = m_target.position;
            m_currentLookAt = m_target;
            m_currentStateId = 0;
        }

        private void LateUpdate()
        {
            Vector3 lookAt = m_currentLookAt.position;
            lookAt.y += m_lookAtOffset;
            if (!m_isTranslating && m_followPlayer)
            {
                if (m_constraints.m_positionBoundaries.canMove)
                {
                    Vector3 deltaPosition = m_target.position - m_targetLastPosition;
                    float x = 0.0f;
                    float y = 0.0f;
                    float z = 0.0f;
                    bool positionChanged = false;

                    float currentDistX = Mathf.Abs(m_constraints.m_positionBoundaries.triggerCenter.x - (deltaPosition.x + transform.position.x));
                    if ((currentDistX <= m_constraints.m_positionBoundaries.bounds.x) || !m_constraints.m_freezePosition.x)
                    {
                        x = deltaPosition.x;
                        positionChanged = true;
                    }
                    float currentDistY = Mathf.Abs(m_constraints.m_positionBoundaries.triggerCenter.y - (deltaPosition.x + transform.position.y));
                    if ((currentDistY <= m_constraints.m_positionBoundaries.bounds.y) || !m_constraints.m_freezePosition.y)
                    {
                        y = deltaPosition.y;
                        positionChanged = true;
                    }
                    float currentDistZ = Mathf.Abs(m_constraints.m_positionBoundaries.triggerCenter.z - (deltaPosition.x + transform.position.z));
                    if ((currentDistZ <= m_constraints.m_positionBoundaries.bounds.z) || !m_constraints.m_freezePosition.z)
                    {
                        z = deltaPosition.z;
                        positionChanged = true;
                    }
                    transform.position = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
                    
                    if (positionChanged)
                    {
                        Vector3 currentLookAt = transform.position + transform.forward;
                        x = lookAt.x;
                        y = lookAt.y;
                        z = lookAt.z;
                        if (m_constraints.m_freezeRotation.x)
                        {
                            x = currentLookAt.x;
                        }
                        if (m_constraints.m_freezeRotation.y)
                        {
                            y = currentLookAt.y;
                        }
                        if (m_constraints.m_freezeRotation.z)
                        {
                            z = currentLookAt.z;
                        }
                        lookAt = new Vector3(x, y, z);
                    }
                }
            }
            transform.LookAt(lookAt);
            m_targetLastPosition = m_target.position;
        }

        private IEnumerator MoveToPosition(Vector3 targetPosition, float waitTime = 1.5f)
        {
            float elapsedTime = 0.0f;
            Vector3 currentPosition = transform.position;
            m_isTranslating = true;

            while (elapsedTime < waitTime)
            {
                transform.position = Vector3.LerpUnclamped(currentPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Make sure we got there
            transform.position = targetPosition;
            m_isTranslating = false;
            yield return null;
        }
    }
}
