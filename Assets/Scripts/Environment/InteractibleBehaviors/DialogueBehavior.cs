using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Player;
using GameCamera;
using AI.Behavior;
using AI.Personality;

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
        private float m_cameraTransitionWaitTime = 1.0f;

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

        private CameraPositionController m_cameraPositionController;
        private InputController m_inputController;
        private Interactible m_interactible;
        private bool m_isActive = false;
        private Transform m_oldLookAtPosition;
        private Vector3 m_oldPosition;
        private BehaviorManager[] m_behaviorManagers;
        private PersonalityManager[] m_personalityManagers;

        private int m_currentMessageIndex = 0;

        public void ShowNextMessage()
        {
            if (m_currentMessageIndex >= m_messages.Length)
            {
                for (int i = 0; i < m_behaviorManagers.Length; i++)
                {
                    if (m_behaviorManagers[i])
                    {
                        m_behaviorManagers[i].ContinueBehavior();
                    }
                    if (m_personalityManagers[i])
                    {
                        m_personalityManagers[i].ContinueBehavior();
                    }
                }
                m_cameraPositionController.lookAtPosition = m_oldLookAtPosition;
                m_cameraPositionController.SetPosition(m_oldPosition);
                DeactivateDialogueBehavior();
                return;
            }

            DialogueMessage currentMessage = m_messages[m_currentMessageIndex];
            int currentSpeaker = currentMessage.m_currentSpeaker;
            
            // set camera position in front of target and look at target
            m_cameraPositionController.lookAtPosition = m_discussionTargets[currentSpeaker];
            Vector3 cameraPosition = 
                m_discussionTargets[currentSpeaker].position
                + 
                (
                new Vector3(m_discussionTargets[currentSpeaker].forward.x, m_discussionTargets[currentSpeaker].forward.y + 2.0f, m_discussionTargets[currentSpeaker].forward.z) 
                * m_cameraPositionController.lookAtOffset
                );
            m_cameraPositionController.SetPosition(cameraPosition, m_cameraTransitionWaitTime);

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
            m_cameraPositionController = FindObjectOfType(typeof(CameraPositionController)) as CameraPositionController;
            m_behaviorManagers = new BehaviorManager[m_discussionTargets.Length];
            m_personalityManagers = new PersonalityManager[m_discussionTargets.Length];
            for (int i = 0; i < m_discussionTargets.Length; i++)
            {
                BehaviorManager behaviorManager = m_discussionTargets[i].GetComponentInChildren(typeof(BehaviorManager)) as BehaviorManager;
                if (behaviorManager)
                {
                    m_behaviorManagers[i] = behaviorManager;
                }
                PersonalityManager personalityManager = m_discussionTargets[i].GetComponentInChildren(typeof(PersonalityManager)) as PersonalityManager;
                if (personalityManager)
                {
                    m_personalityManagers[i] = personalityManager;
                }
            }
        }

        private void Update()
        {
            if (m_interactible.isActive)
            {
                if (!m_isActive)
                {
                    for (int i = 0; i < m_behaviorManagers.Length; i++)
                    {
                        if (m_behaviorManagers[i])
                        {
                            m_behaviorManagers[i].InterruptBehavior();
                        }
                        if (m_personalityManagers[i])
                        {
                            m_personalityManagers[i].InterruptBehavior();
                        }
                    }
                    m_oldLookAtPosition = m_cameraPositionController.lookAtPosition;
                    m_oldPosition = m_cameraPositionController.currentPosition;
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
