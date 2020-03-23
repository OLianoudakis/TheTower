using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.Inventory;
using Player;

namespace Environment.InteractibleBehaviors
{
    [System.Serializable]
    public struct POIMessage
    {
        public string m_note;
        public Item m_givesItem;
        public int m_givesItemQuantity;
        public Item m_requiresItem;
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
        private Animator m_animator;

        [SerializeField]
        private bool m_discardObject = false;

        private InputController m_inputController;
        private PlayerInventoryController m_playerInventoryController;
        private Interactible m_interactible;
        private bool m_isActive = false;
        private int m_currentMessage = 0;
        private string m_infoMessage;

        public int GetNumberOfMessages()
        {
            return m_messages.Length;
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
                m_infoMessage = "You've received " + m_messages[m_currentMessage].m_givesItemQuantity.ToString() + " " + m_messages[m_currentMessage].m_givesItem.m_itemName + ".";
                StartCoroutine(RepeatingMessage());
            }

            bool canMoveForward = true;
            if (m_messages[m_currentMessage].m_requiresItem)
            {
                PlayerInventoryController playerInventoryController = FindObjectOfType(typeof(PlayerInventoryController)) as PlayerInventoryController;
                if (playerInventoryController)
                {
                    m_messages[m_currentMessage].m_requiredItemQuantity 
                        = playerInventoryController.RemoveItem(m_messages[m_currentMessage].m_requiresItem.m_itemType, m_messages[m_currentMessage].m_requiredItemQuantity);
                }
                
                //Requirements met, activate animation
                if (m_messages[m_currentMessage].m_requiredItemQuantity <= 0)
                {
                    m_messages[m_currentMessage].m_note = "There!";
                    PlayAnimation();
                }
                else
                {
                    m_infoMessage = "It needs " + m_messages[m_currentMessage].m_requiredItemQuantity + " " + m_messages[m_currentMessage].m_requiresItem.m_itemName;
                    m_messages[m_currentMessage].m_note = m_infoMessage;
                    canMoveForward = false;
                }
            }

            if (m_messages[m_currentMessage].m_requiresItem && m_messages[m_currentMessage].m_requiresItem.m_itemType == ItemType.AutomaticPass)
            {
                PlayAnimation();
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

        private void PlayAnimation()
        {
            if (m_animator)
            {
                m_animator.SetInteger("AnimationState", 1);
            }
        }

        private void DeactivatePOIBehavior(bool permanent)
        {
            StartCoroutine(DeactivatePOIBehaviorCourutine());
            m_dialogueGroup.alpha = 0.0f;
            if (permanent)
            {
                if (m_discardObject)
                {
                    gameObject.SetActive(false);
                }
                m_interactible.DeactivateBehavior(true);
                this.enabled = false;
            }
            m_interactible.DeactivateBehavior(false);
        }

        private IEnumerator DeactivatePOIBehaviorCourutine()
        {
            yield return new WaitForEndOfFrame();
        }

        private void Start()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
            m_playerInventoryController = FindObjectOfType(typeof(PlayerInventoryController)) as PlayerInventoryController;
        }

        private void Update()
        {
            if (m_interactible.isActive)
            {
                if (!m_isActive)
                {
                    m_isActive = true;
                    ShowNextMessage();
                }
                else if(m_inputController.isLeftMouseClick)
                {
                    ShowNextMessage();
                }
            }
        }
    }
}
