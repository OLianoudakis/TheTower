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
        [SerializeField]
        private float m_fadeLerpSpeed = 2.0f;
        private float m_fadeLerpInterval = 0.0f;
        private bool m_isInvisible;

        public bool isInvisible
        {
            get { return m_isInvisible; }
        }

        public void SetInvisible()
        {
            m_isInvisible = true;
            StartCoroutine(ShadowUIFade(true));
        }

        public void SetVisible()
        {
            m_isInvisible = false;
            StartCoroutine(ShadowUIFade(false));
        }

        private void Start()
        {
            m_isInvisible = false;
        }

        private IEnumerator ShadowUIFade(bool fadeIn)
        {
            bool currentlyFading = true;
            if (fadeIn)
            {
                while (currentlyFading)
                {
                    m_invisibilityCanvas.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_invisibilityCanvas.alpha = 1.0f;
                        m_fadeLerpInterval = 0.0f;
                        currentlyFading = false;
                        yield return null;
                    }
                }
            }
            else
            {
                while (currentlyFading)
                {
                    m_invisibilityCanvas.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_invisibilityCanvas.alpha = 0.0f;
                        m_fadeLerpInterval = 0.0f;
                        currentlyFading = false;
                        yield return null;
                    }
                }
            }
        }
    }
}
