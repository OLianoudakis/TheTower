using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace POI
{
    public class POIDetector : MonoBehaviour
    {
        [SerializeField]
        private InputController m_inputController;

        private Vector3 m_lastMousePosition = Vector3.zero;
        private POIBehavior m_poiBehavior;

        public POIBehavior poiBehavior
        {
            get { return m_poiBehavior; }
        }

        private void Update()
        {
            if (m_inputController.isLeftMouseClick && (m_lastMousePosition != m_inputController.leftMouseClickPosition) && m_inputController.leftMouseClickHit.collider)
            {
                POIBehavior poiBehavior = null;
                if (m_inputController.leftMouseClickHit.collider.gameObject.tag.Equals("POI"))
                {
                    poiBehavior = m_inputController.leftMouseClickHit.collider.gameObject.GetComponent(typeof(POIBehavior)) as POIBehavior;
                }
                m_poiBehavior = poiBehavior;
            }
        }
    }
}
