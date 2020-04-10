using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndGroup : MonoBehaviour
{
    [SerializeField]
    private CreditsFader m_firstCredit;
    private CanvasGroup m_blackScreenCanvas;

    public void BeginCredits()
    {
        m_blackScreenCanvas.alpha = 1.0f;
        StartCoroutine(FirstCreditDelay());
    }

    private void Awake()
    {
        m_blackScreenCanvas = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
    }

    private IEnumerator FirstCreditDelay()
    {
        yield return new WaitForSeconds(1.0f);
        m_firstCredit.BeginFade();
    }
}
