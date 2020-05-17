using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class HideGroup : MonoBehaviour
    {
        [SerializeField]
        private float m_fadeLerpSpeed = 2.0f;
        private float m_fadeLerpInterval = 0.0f;
        private CanvasGroup m_creditCanvasGroup;

        private bool m_currentlyFading = false;
        private bool m_fadeIn;

        public void BeginFade(bool fadeIn)
        {
            m_currentlyFading = true;
            m_fadeIn = fadeIn;
        }

        private void Awake()
        {
            m_creditCanvasGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
        }

        private void Update()
        {
            if (m_currentlyFading)
            {

                if (m_fadeIn)
                {
                    m_creditCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_creditCanvasGroup.alpha = 1.0f;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                    }
                }
                else
                {
                    m_creditCanvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_creditCanvasGroup.alpha = 0.0f;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                    }
                }
            }
        }
    }

}

