using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Environment
{
    public class InteractibleDetector : MonoBehaviour
    {
        [SerializeField]
        private InputController m_inputController;

        private Vector3 m_lastMousePosition = Vector3.zero;
        private Interactible m_interactible;

        public Interactible interactible
        {
            get { return m_interactible; }
        }

        private void Update()
        {
            if (m_inputController.isLeftMouseClick && (m_lastMousePosition != m_inputController.leftMouseClickPosition) && m_inputController.leftMouseClickHit.collider)
            {
                Interactible interactible = null;
                if (m_inputController.leftMouseClickHit.collider.gameObject.tag.Equals("Interactible"))
                {
                    interactible = m_inputController.leftMouseClickHit.collider.gameObject.GetComponent(typeof(Interactible)) as Interactible;
                }
                m_interactible = interactible;
            }
        }
    }
}
