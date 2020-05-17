using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUI
{
    public class ShowCanvasGroup : MonoBehaviour
    {
        [SerializeField]
        private float m_fadeLerpSpeed = 2.0f;
        [SerializeField]
        private CanvasGroup m_canvasGroup;

        private bool m_fadeIn;
        private bool m_currentlyFading = false;
        private float m_fadeLerpInterval = 0.0f;

        public void ShowHideCanvasGroup(bool fadeIn)
        {
            m_currentlyFading = true;
            m_fadeIn = fadeIn;
        }

        private void Update()
        {
            if (m_currentlyFading)
            {
                if (m_fadeIn)
                {
                    m_canvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_canvasGroup.alpha = 1.0f;
                        m_canvasGroup.blocksRaycasts = true;
                        m_canvasGroup.interactable = true;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                    }
                }
                else
                {
                    m_canvasGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_canvasGroup.alpha = 0.0f;
                        m_canvasGroup.blocksRaycasts = false;
                        m_canvasGroup.interactable = false;
                        m_fadeLerpInterval = 0.0f;
                        m_currentlyFading = false;
                    }
                }
            }
        }
    }
}