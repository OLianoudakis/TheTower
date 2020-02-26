using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POIBehavior : MonoBehaviour
{
    [System.Serializable]
    public class UIMessage
    {
        public string m_note;

        public ItemOfInterest m_item;

        public bool m_requiresItem;
        public ItemType m_itemType;
        public string m_itemTypeName;
        public int m_itemQuantity;
    };

    public CanvasGroup m_dialogueGroup;
    public Text m_dialogueWindow;
    public CanvasGroup m_infoGroup;
    public Text m_infoText;    

    /*public string[] m_notes;
    public bool m_hasItemsToGive;
    public GameObject[] m_items;*/

    public UIMessage[] m_messages;

    [SerializeField]
    private Transform m_poiPositionTarget;
    [SerializeField]
    private Animator m_animator;

    private PlayerDialogueController m_playerDialogueController;
    private int m_currentMessage = 0;
    private bool m_canMoveForward = true;
    private string m_infoMessage;

    public void ActivatePOIBehavior(PlayerDialogueController playerDialogueController)
    {
        m_playerDialogueController = playerDialogueController;
        ShowNextMessage(); //show first message
        
    }

    public void ShowNextMessage()
    {
        if (m_currentMessage >= m_messages.Length)
        {
            StartCoroutine(DeactivatePOI());
            return;
        }
        
        //Hide previous info
        m_infoGroup.alpha = 0.0f;

        if (m_messages[m_currentMessage].m_item)
        {
            FindObjectOfType<PlayerInventoryController>().AddItem(m_messages[m_currentMessage].m_item);
            m_infoMessage = "You've received " + m_messages[m_currentMessage].m_item.GetQuantity().ToString() + " " + m_messages[m_currentMessage].m_item.GetItemName() + ".";
            StartCoroutine(RepeatingMessage());
        }

        if (m_messages[m_currentMessage].m_requiresItem)
        {
            m_canMoveForward = false;
            m_messages[m_currentMessage].m_itemQuantity = FindObjectOfType<PlayerInventoryController>().FindItemOfType(m_messages[m_currentMessage].m_itemType, m_messages[m_currentMessage].m_itemQuantity);
            
            //Requirements met, activate animation
            if (m_messages[m_currentMessage].m_itemQuantity <= 0)
            {
                m_messages[m_currentMessage].m_note = "There!";
                ActivateAnimation(1);
                m_canMoveForward = true;
            }
            else
            {
                m_infoMessage = "It needs " + m_messages[m_currentMessage].m_itemQuantity + " " + m_messages[m_currentMessage].m_itemTypeName;
                m_messages[m_currentMessage].m_note = m_infoMessage;
            }
        }

        //Display message
        m_dialogueWindow.text = m_messages[m_currentMessage].m_note;

        if (m_canMoveForward)
        {
            ++m_currentMessage;
        }
        else
        {
            StartCoroutine(DeactivatePOI());
            StartCoroutine(RepeatingMessage());
            return;
        }

        m_dialogueGroup.alpha = 1.0f;
    }

    public int GetNumberOfMessages()
    {
        return m_messages.Length;
    }

    public Vector3 GetPOIDestinationPoint()
    {
        return m_poiPositionTarget.position;
    }

    public void DeactivatePOIBehavior()
    {
        m_dialogueGroup.alpha = 0.0f;
        m_playerDialogueController.SwitchToMovement();
    }

    public void ActivateAnimation(int state)
    {
        m_animator.SetInteger("AnimationState", state);
    }

    private IEnumerator DeactivatePOI()
    {
        yield return new WaitForEndOfFrame();
        DeactivatePOIBehavior();
    }

    private IEnumerator RepeatingMessage()
    {
        m_infoGroup.alpha = 1.0f;
        m_infoText.text = m_infoMessage;
        yield return new WaitForSeconds(2.0f);
        m_infoGroup.alpha = 0.0f;
    }
}
