using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsFader : MonoBehaviour
{
    [SerializeField]
    private CreditsFader m_nextCredit;
    [SerializeField]
    private float m_fadeLerpSpeed = 2.0f;
    [SerializeField]
    private bool m_finalCredit = false;
    [SerializeField]
    private bool m_firstCredit = false;

    private float m_fadeLerpInterval = 0.0f;
    private CanvasGroup m_creditCanvasGroup;

    private bool m_currentlyFading = false;
    private bool m_fadeIn;

    public void BeginFade()
    {
        m_currentlyFading = true;
    }

    private void BeginNextCredit()
    {
        if (m_finalCredit)
        {
            Debug.Log("it ended");
            Application.Quit();
            return;
        }
        m_nextCredit.BeginFade();
    }

    private void Awake()
    {
        m_creditCanvasGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
    }

    private void Update()
    {
        if (m_currentlyFading)
        {
            if (m_finalCredit)
            {
                m_creditCanvasGroup.interactable = true;
                m_creditCanvasGroup.blocksRaycasts = true;
            }
            if (m_fadeIn)
            {
                m_creditCanvasGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_creditCanvasGroup.alpha = 1.0f;
                    m_fadeLerpInterval = 0.0f;
                    if (!m_finalCredit && !m_firstCredit)
                    {
                        m_fadeIn = false;
                    }
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
                    BeginNextCredit();
                    m_currentlyFading = false;
                }
            }                     
        }
    }   
}