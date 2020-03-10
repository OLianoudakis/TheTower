using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerInvisibility : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_invisibilityCanvas;

        private bool m_isInvisible;

        public bool isInvisible
        {
            get { return m_isInvisible; }
        }

        public void SetInvisible()
        {
            m_isInvisible = true;
            m_invisibilityCanvas.alpha = 1.0f;
        }

        public void SetVisible()
        {
            m_isInvisible = false;
            m_invisibilityCanvas.alpha = 0.0f;
        }

        private void Start()
        {
            m_isInvisible = false;
        }
    }
}
