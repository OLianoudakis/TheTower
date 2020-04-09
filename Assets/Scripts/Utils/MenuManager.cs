using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_fadeBlackGroup;
    [SerializeField]
    private CanvasGroup m_iconAndTitleGroup;
    [SerializeField]
    private CanvasGroup m_buttonGroup;

    private float m_fadeLerpInterval = 0.0f;
    private float m_fadeLerpSpeed = 0.1f;
    private bool m_currentlyFading = true;

    private void Start()
    {
        StartCoroutine(FirstFadeIn());
    }

    private void FadeInTitle()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInTitleCoroutine());
    }

    private void FadeInButton()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInButtonCoroutine());
    }

    private IEnumerator FirstFadeIn()
    {
        while (m_currentlyFading)
        {
            m_fadeBlackGroup.alpha = Mathf.Lerp(1.0f, 0.0f, Mathf.SmoothStep(0.0f, 1.0f, m_fadeLerpInterval));
            m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (m_fadeLerpInterval > 1.0f)
            {
                m_fadeBlackGroup.alpha = 0.0f;
                m_fadeLerpInterval = 0.0f;
                m_fadeLerpSpeed = 0.3f;
                m_currentlyFading = false;
                FadeInTitle();
                yield return null;
            }
        }
    }

    private IEnumerator FadeInTitleCoroutine()
    {
        m_currentlyFading = true;
        while (m_currentlyFading)
        {
            m_iconAndTitleGroup.alpha = Mathf.Lerp(0.0f, 1.0f, Mathf.SmoothStep(0.0f, 1.0f, m_fadeLerpInterval));
            m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (m_fadeLerpInterval > 1.0f)
            {
                m_iconAndTitleGroup.alpha = 1.0f;
                m_fadeLerpInterval = 0.0f;
                m_fadeLerpSpeed = 0.5f;
                m_currentlyFading = false;
                FadeInButton();
                yield return null;
            }
        }
    }

    private IEnumerator FadeInButtonCoroutine()
    {
        m_currentlyFading = true;
        m_buttonGroup.interactable = true;
        m_buttonGroup.blocksRaycasts = true;
        while (m_currentlyFading)
        {
            m_buttonGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
            m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (m_fadeLerpInterval > 1.0f)
            {
                m_buttonGroup.alpha = 1.0f;                
                m_fadeLerpInterval = 0.0f;
                m_currentlyFading = false;
                yield return null;
            }
        }
    }
}
