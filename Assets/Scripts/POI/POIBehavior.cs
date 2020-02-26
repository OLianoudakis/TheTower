using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POIBehavior : MonoBehaviour
{
    public CanvasGroup m_canvasGroup;
    public Text m_dialogueWindow;

    public string m_note;

    public void ActivatePOIBehavior()
    {
        m_dialogueWindow.text = m_note;
        m_canvasGroup.alpha = 1.0f;
    }

    public void DeactivatePOIBehavior()
    {
        m_canvasGroup.alpha = 0.0f;
    }
}
