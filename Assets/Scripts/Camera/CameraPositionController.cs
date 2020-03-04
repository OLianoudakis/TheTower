using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class CameraPositionController : MonoBehaviour
    {
        public CameraCCTVState m_currentState;

        [SerializeField]
        private Transform m_playerPosition;
        [SerializeField]
        private Vector3 m_offset = new Vector3(-3.0f, 7.0f, -3.0f);

        public void SetPosition(Vector3 targetPosition)
        {
            StartCoroutine(MoveToPosition(targetPosition));
        }

        public void SetPosition(Vector3 targetPosition, CameraCCTVState newState)
        {
            m_currentState = newState;
            StartCoroutine(MoveToPosition(targetPosition));
        }

        public CameraCCTVState CameraState
        {
            get { return m_currentState; }
        }

        private void Start()
        {
            m_currentState = CameraCCTVState.CCTV1;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            //transform.position = m_playerPosition.position + m_offset;
            transform.LookAt(m_playerPosition);
        }

        private IEnumerator MoveToPosition(Vector3 targetPosition)
        {
            float elapsedTime = 0.0f;
            float waitTime = 1.5f;
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
