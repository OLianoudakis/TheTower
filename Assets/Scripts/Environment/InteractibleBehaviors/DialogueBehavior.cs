using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;

namespace Environment.InteractibleBehaviors
{
    [System.Serializable]
    public struct DialogueMessage
    {
        public string m_messageText;
        public int m_currentSpeaker;
    };

    public class DialogueBehavior : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup m_dialogueGroup;

        [SerializeField]
        private Text m_dialogueWindow;

        [SerializeField]
        private Image m_characterIcon;

        [SerializeField]
        private Transform[] m_discussionTargets;

        [SerializeField]
        private Sprite[] m_discussionTargetsIcons;

        [SerializeField]
        private DialogueMessage[] m_messages;

        private InputController m_inputController;
        private Interactible m_interactible;
        private bool m_isActive = false;

        private int m_currentMessageIndex = 0;

        public void ShowNextMessage()
        {
            if (m_currentMessageIndex >= m_messages.Length)
            {
                DeactivateDialogueBehavior();
                return;
            }

            DialogueMessage currentMessage = m_messages[m_currentMessageIndex];
            int currentSpeaker = currentMessage.m_currentSpeaker;
            // get the current message target
            // Transform targetTransform = m_discussionTargets[currentSpeaker];
            // TODO here set the camera to its position

            // change the icon for the target
            Sprite targetIcon = m_discussionTargetsIcons[currentSpeaker];
            m_characterIcon.sprite = targetIcon;

            //Display message
            m_dialogueWindow.text = currentMessage.m_messageText;

            ++m_currentMessageIndex;
            m_dialogueGroup.alpha = 1.0f;
        }

        private void DeactivateDialogueBehavior()
        {
            StartCoroutine(DeactivateDialogueBehaviorCourutine());
            m_dialogueGroup.alpha = 0.0f;
            m_interactible.DeactivateBehavior(true);
            this.enabled = false;
        }

        private IEnumerator DeactivateDialogueBehaviorCourutine()
        {
            yield return new WaitForEndOfFrame();
        }

        private void Start()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
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
                else if (m_inputController.isLeftMouseClick)
                {
                    ShowNextMessage();
                }
            }
        }
    }
}
