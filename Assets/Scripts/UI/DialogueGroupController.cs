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

    private void Awake()
    {
        m_dialogueGroup = GetComponent(typeof(CanvasGroup)) as CanvasGroup;
        m_dialogueText = GetComponentInChildren(typeof(Text)) as Text;
    }

    public void ShowDialogueWindow()
    {
        StartCoroutine(DialogueUIFade(true));
    }

    public void HideDialogueWindow()
    {
        StartCoroutine(DialogueUIFade(false));
    }

    public void ChangeText(string text)
    {
        m_dialogueText.text = text;
    }

    private IEnumerator DialogueUIFade(bool fadeIn)
    {
        bool currentlyFading = true;
        if (fadeIn)
        {
            while (currentlyFading)
            {
                m_dialogueGroup.alpha = Mathf.Lerp(0.0f, 1.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_dialogueGroup.alpha = 1.0f;
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
                m_dialogueGroup.alpha = Mathf.Lerp(1.0f, 0.0f, m_fadeLerpInterval);
                m_fadeLerpInterval += m_fadeLerpSpeed * Time.unscaledDeltaTime;
                yield return new WaitForEndOfFrame();
                if (m_fadeLerpInterval > 1.0f)
                {
                    m_dialogueGroup.alpha = 0.0f;
                    m_fadeLerpInterval = 0.0f;
                    currentlyFading = false;
                    yield return null;
                }
            }
        }
    }
}
