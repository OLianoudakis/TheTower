using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class CCTVPositionController : MonoBehaviour
    {
        private CameraPositionController m_mainCamera;

        public void ChangeCameraPosition(Vector3 CCTVPosition, uint stateId, bool followPlayer = false, Constraints constraints = null)
        {
            if (stateId != m_mainCamera.cameraStateId)
            {
                m_mainCamera.SetPosition(CCTVPosition, stateId, followPlayer: followPlayer, constraints: constraints);
            }
        }

        private void Start()
        {
            m_mainCamera = FindObjectOfType<CameraPositionController>();
        }
    }
}
