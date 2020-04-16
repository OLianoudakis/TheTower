using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class InfoGroupController : MonoBehaviour
    {
        [SerializeField]
        private float m_fadeLerpSpeed = 0.5f;

        private float m_fadeLerpInterval = 0.0f;

        private CanvasGroup m_infoGroup;
        private Text m_infoGroupText;

        public void SpawnInfoGroup(string message)
        {
            m_infoGroupText.text = message;
            FadeInfoGroup(true);
        }

        public void HidePreviousInfo()
        {
            StopAllCoroutines();
            m_infoGroup.alpha = 0.0f;
        }

        private void Awake()
        {
            m_infoGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
            m_infoGroupText = GetComponentInChildren(typeof(Text)) as Text;
        }

        private void FadeInfoGroup(bool fadeIn)
        {
            StartCoroutine(InfoUIFade(fadeIn));
        }

        private IEnumerator InfoUIFade(bool fadeIn)
        {
            bool currentlyFading = true;
            if (fadeIn)
            {
                while (currentlyFading)
                {
                    m_infoGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_infoGroup.alpha = 1.0f;
                        m_fadeLerpInterval = 0.0f;
                        currentlyFading = false;
                        FadeInfoGroup(false);
                        yield return null;
                    }
                }
            }
            else
            {
                while (currentlyFading)
                {
                    m_infoGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                    m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                    if (m_fadeLerpInterval > 1.0f)
                    {
                        m_infoGroup.alpha = 0.0f;
                        m_fadeLerpInterval = 0.0f;
                        currentlyFading = false;
                        yield return null;
                    }
                }
            }
        }
    }

}
