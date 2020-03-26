using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class CameraPositionController : MonoBehaviour
    {
        public CameraCCTVState m_currentState;

        [SerializeField]
        private Transform m_lookAtPosition;

        [SerializeField]
        private float m_lookAtOffset = 3.0f;

        private Vector3 m_currentPosition;

        public Transform lookAtPosition
        {
            get { return m_lookAtPosition; }
            set { m_lookAtPosition = value; }
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

        public void SetPosition(Vector3 targetPosition, CameraCCTVState newState, float waitTime = 1.5f)
        {
            m_currentPosition = targetPosition;
            m_currentState = newState;
            StartCoroutine(MoveToPosition(targetPosition, waitTime));
        }

        public CameraCCTVState CameraState
        {
            get { return m_currentState; }
        }

        private void Start()
        {
            m_currentState = CameraCCTVState.CCTV1;
        }

        private void LateUpdate()
        {
            transform.LookAt(m_lookAtPosition);
        }

        private IEnumerator MoveToPosition(Vector3 targetPosition, float waitTime = 1.5f)
        {
            float elapsedTime = 0.0f;
            Vector3 currentPosition = transform.position;

            while (elapsedTime < waitTime)
            {
                transform.position = Vector3.LerpUnclamped(currentPosition, targetPosition, Mathf.SmoothStep(0.0f, 1.0f, (elapsedTime / waitTime)));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // Make sure we got there
            transform.position = targetPosition;
            yield return null;
        }
    }

    public enum CameraCCTVState { CCTV1, CCTV2, CCTV3, CCTV4, CCTV5, CCTV6, CCTV7, CCTV8 , CCTV9, CCTV10, CCTV11, CCTV12, CCTV13, CCTV14, CCTV15, CCTV16, CCTV17, CCTV18, CCTV19, CCTV20, CCTV21, CCTV22, CCTV23 }
}
