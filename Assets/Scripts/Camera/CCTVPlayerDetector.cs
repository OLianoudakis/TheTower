using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameCamera
{
    public class CCTVPlayerDetector : MonoBehaviour
    {
        [SerializeField]
        private CameraCCTVState m_CCTVState; //set in inspector
        [SerializeField]
        private Transform m_CCTVPosition;

        private CCTVPositionController m_positionController;

        private void Start()
        {
            m_positionController = FindObjectOfType<CCTVPositionController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_positionController.ChangeCameraPosition(m_CCTVPosition.position, m_CCTVState);
            }
        }
    }
}
