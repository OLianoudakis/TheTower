using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class CCTVPositionController : MonoBehaviour
    {
        private CameraPositionController m_mainCamera;

        public void ChangeCameraPosition(Vector3 CCTVPosition, CameraCCTVState CCTVStateName)
        {
            if (CCTVStateName != m_mainCamera.CameraState)
            {
                m_mainCamera.SetPosition(CCTVPosition, CCTVStateName);
            }
        }

        private void Start()
        {
            m_mainCamera = FindObjectOfType<CameraPositionController>();
        }
    }
}
