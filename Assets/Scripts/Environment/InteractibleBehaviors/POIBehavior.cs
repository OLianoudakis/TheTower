using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player.Inventory;
using Player;
using Tutorial;

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
        private TutorialManager m_tutorialManager;

        [SerializeField]
        private Text m_repeatedInfoText;

        [SerializeField]
        private POIMessage[] m_messages;

        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private bool m_discardObject = false;

        [SerializeField]
        private AudioClip m_initialSound;

        [SerializeField]
        private AudioClip m_animationSound;

        private InputController m_inputController;
        private PlayerInventoryController m_playerInventoryController;
        private Interactible m_interactible;
        private InfoGroupController m_infoGroup;
        private DialogueGroupController m_dialogueGroup;
        private AudioSource m_poiAudioSource;

        private bool m_isActive = false;
        private int m_currentMessage = 0;
        private string m_infoMessage;

        public int GetNumberOfMessages()
        {
            return m_messages.Length;
        }

        public void ShowNextMessage()
        {
            if (m_currentMessage >= m_messages.Length)
            {
                //TUTORIAL SECTION
                if (gameObject.name.Equals("Book_Open"))
                {
                    m_tutorialManager.StepCompleted();
                }
                //END OF TUTORIAL SECTION
                DeactivatePOIBehavior(true);
                return;
            }

            //Hide previous info
            m_infoGroup.HidePreviousInfo();

            if (m_messages[m_currentMessage].m_givesItem)
            {
                m_playerInventoryController.AddItem(m_messages[m_currentMessage].m_givesItem, m_messages[m_currentMessage].m_givesItemQuantity);
                m_infoMessage = "You've received " + m_messages[m_currentMessage].m_givesItemQuantity.ToString() + " " + m_messages[m_currentMessage].m_givesItem.m_itemName + ".";
                m_infoGroup.SpawnInfoGroup(m_infoMessage);
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
            m_dialogueGroup.ChangeText(m_messages[m_currentMessage].m_note);

            if (canMoveForward)
            {
                ++m_currentMessage;
            }
            else
            {
                DeactivatePOIBehavior(false);
                m_infoGroup.SpawnInfoGroup(m_infoMessage);
                return;
            }

            m_dialogueGroup.ShowDialogueWindow();
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
            m_dialogueGroup.HideDialogueWindow();
            if (permanent)
            {
                if (m_discardObject)
                {
                    gameObject.SetActive(false);
                }
                m_interactible.DeactivateBehavior(true);
                this.enabled = false;
                return;
            }
            m_interactible.DeactivateBehavior(false);
        }

        private IEnumerator DeactivatePOIBehaviorCourutine()
        {
            yield return new WaitForEndOfFrame();
        }

        private void Awake()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
            m_playerInventoryController = FindObjectOfType(typeof(PlayerInventoryController)) as PlayerInventoryController;
            m_infoGroup = FindObjectOfType(typeof(InfoGroupController)) as InfoGroupController;
            m_dialogueGroup = FindObjectOfType(typeof(DialogueGroupController)) as DialogueGroupController;
            m_poiAudioSource = GetComponent(typeof(AudioSource)) as AudioSource;
        }

        private void Update()
        {
            if (m_interactible.isActive)
            {
                if (!m_isActive)
                {
                    m_poiAudioSource.PlayOneShot(m_initialSound);
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
