using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct POIMessage
{
    public string m_note;
    public ItemOfInterest m_givesItem;
    public int m_givesItemQuantity;
    public ItemType m_requiredItemType;
    public string m_requiredItemTypeName;
    public int m_requiredItemQuantity;
};

public class POIBehavior : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup m_dialogueGroup;

    [SerializeField]
    private Text m_dialogueWindow;

    [SerializeField]
    private CanvasGroup m_infoGroup;

    [SerializeField]
    private Text m_repeatedInfoText;

    [SerializeField]
    private POIMessage[] m_messages;

    [SerializeField]
    private Transform m_poiPositionTarget;

    [SerializeField]
    private Animator m_animator;

    [SerializeField]
    private PlayerInventoryController m_playerInventoryController;

    private bool m_isActive = false;
    private int m_currentMessage = 0;
    private string m_infoMessage;

    public bool isActive
    {
        get { return m_isActive; }
    }

    public void ActivatePOIBehavior()
    {
        if (m_currentMessage < m_messages.Length)
        {
            m_isActive = true;
        }
    }

    public int GetNumberOfMessages()
    {
        return m_messages.Length;
    }

    public Vector3 GetPOIDestinationPoint()
    {
        return m_poiPositionTarget.position;
    }

    private IEnumerator RepeatingMessage()
    {
        m_infoGroup.alpha = 1.0f;
        m_repeatedInfoText.text = m_infoMessage;
        yield return new WaitForSeconds(2.0f);
        m_infoGroup.alpha = 0.0f;
    }

    public void ShowNextMessage()
    {
        if (!m_isActive)
        {
            return;
        }

        if (m_currentMessage >= m_messages.Length)
        {
            DeactivatePOIBehavior(true);
            return;
        }
        
        //Hide previous info
        m_infoGroup.alpha = 0.0f;

        if (m_messages[m_currentMessage].m_givesItem)
        {
            m_playerInventoryController.AddItem(m_messages[m_currentMessage].m_givesItem, m_messages[m_currentMessage].m_givesItemQuantity);
            m_infoMessage = "You've received " + m_messages[m_currentMessage].m_givesItemQuantity.ToString() + " " + m_messages[m_currentMessage].m_givesItem.GetItemName() + ".";
            StartCoroutine(RepeatingMessage());
        }

        bool canMoveForward = true;
        if (m_messages[m_currentMessage].m_requiredItemType != ItemType.None)
        {
            m_messages[m_currentMessage].m_requiredItemQuantity = FindObjectOfType<PlayerInventoryController>().FindItemOfType(m_messages[m_currentMessage].m_requiredItemType, m_messages[m_currentMessage].m_requiredItemQuantity);
            
            //Requirements met, activate animation
            if (m_messages[m_currentMessage].m_requiredItemQuantity <= 0)
            {
                m_messages[m_currentMessage].m_note = "There!";
                ActivateAnimation(1);
                
            }
            else
            {
                m_infoMessage = "It needs " + m_messages[m_currentMessage].m_requiredItemQuantity + " " + m_messages[m_currentMessage].m_requiredItemTypeName;
                m_messages[m_currentMessage].m_note = m_infoMessage;
                canMoveForward = false;
            }
        }

        //Display message
        m_dialogueWindow.text = m_messages[m_currentMessage].m_note;

        if (canMoveForward)
        {
            ++m_currentMessage;
        }
        else
        {
            DeactivatePOIBehavior(false);
            StartCoroutine(RepeatingMessage());
            return;
        }

        m_dialogueGroup.alpha = 1.0f;
    }

    private void ActivateAnimation(int state)
    {
        if (m_animator)
        {
            m_animator.SetInteger("AnimationState", state);
        }   
    }

    private void DeactivatePOIBehavior(bool disable)
    {
        StartCoroutine(DeactivatePOIBehaviorCourutine());
        m_dialogueGroup.alpha = 0.0f;
        m_isActive = false;
        if (disable)
        {
            this.enabled = false;
        }
    }

    private IEnumerator DeactivatePOIBehaviorCourutine()
    {
        yield return new WaitForEndOfFrame();
    }
}
