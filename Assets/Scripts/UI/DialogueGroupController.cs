using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueGroupController : MonoBehaviour
{
    [SerializeField]
    private float m_fadeLerpSpeed = 2.0f;

    private CanvasGroup m_dialogueGroup;
    private Text m_dialogueText;
    private float m_fadeLerpInterval = 0.0f;
    private bool m_currentlyFading = false;
    private bool m_fadeIn = false;
    private int m_timesCalled = 0;

    private void Awake()
    {
        m_dialogueGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
        m_dialogueText = GetComponentInChildren(typeof(Text)) as Text;
    }

    public void ShowDialogueWindow()
    {
        //Don't refade after first message
        m_timesCalled++;
        if (m_timesCalled > 0)
        {
            m_dialogueGroup.alpha = 1.0f;
            m_fadeLerpInterval = 0.0f;
            m_currentlyFading = false;
            return;
        }
        m_fadeIn = true;
        m_currentlyFading = true;
    }

    public void HideDialogueWindow()
    {
        m_timesCalled = 0;
        m_fadeIn = false;
        m_currentlyFading = true;
    }

    public void ChangeText(string text)
    {
        m_dialogueText.text = text;
    }

    private void Update()
    {
        if (m_currentlyFading)
        {
            if (m_fadeIn)
            {
                m_dialogueGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_dialogueGroup.alpha = 1.0f;
                    m_fadeLerpInterval = 0.0f;
                    m_currentlyFading = false;
                }
            }
            else
            {
                m_dialogueGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.deltaTime;
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_dialogueGroup.alpha = 0.0f;
                    m_fadeLerpInterval = 0.0f;
                    m_currentlyFading = false;
                }
            }
        }
    }
}
