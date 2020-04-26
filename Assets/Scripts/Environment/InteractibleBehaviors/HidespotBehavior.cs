using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Inventory;
using Player;
using Player.EmptyClass;
using Player.StateHandling.Moving;
using Player.StateHandling.Crouching;
using Tutorial;
using GameUI;

namespace Environment.Hiding
{
    public class HidespotBehavior : MonoBehaviour
    {
        [SerializeField]
        private TutorialManager m_tutorialManager;
        [SerializeField]
        private bool m_inTutorial = false;

        private GameObject m_invisibleWalls;
        private InputController m_inputController;
        private ToCrouching[] m_toCrouchingReferences;
        private Transform m_playerTransform;
        private HideGroup m_hideIconUI;
        private bool m_isHidden = false;
        public bool playerCanHide
        {
            get { return m_playerCanHide; }
            set { m_playerCanHide = value; }
        }

        private bool m_playerCanHide = false;
        private bool m_slidePlayerToCenter = false;

        private void SetPlayerIntoHiding()
        {
            m_invisibleWalls.SetActive(true);
            if (m_inTutorial && !m_isHidden)
            {                
                m_tutorialManager.StepCompleted();
            }
            m_isHidden = true;
        }

        private void ExitHiding()
        {
            m_invisibleWalls.SetActive(false);
        }

        private void Awake()
        {
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
            m_hideIconUI = FindObjectOfType(typeof(HideGroup)) as HideGroup;
            m_toCrouchingReferences = Resources.FindObjectsOfTypeAll<ToCrouching>();
            m_playerTransform = m_inputController.gameObject.transform;
            foreach (Transform child in transform)
            {
                if (child.name == "InvisibleWalls")
                {
                    m_invisibleWalls = child.gameObject;
                    break;
                }
            }
        }

        private void Update()
        {
            if (m_playerCanHide && m_inputController.isRightMouseClick)
            {
                m_playerTransform.position = transform.position;
                SetPlayerIntoHiding();
            }
            else if (m_isHidden && m_inputController.isLeftMouseClick)
            {
                ExitHiding();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_playerCanHide = true;
                for (int i = 0; i < m_toCrouchingReferences.Length; i++)
                {
                    m_hideIconUI.BeginFade(true);
                    m_toCrouchingReferences[i].playerCanHide = true;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_playerCanHide = false;
                for (int i = 0; i < m_toCrouchingReferences.Length; i++)
                {
                    m_hideIconUI.BeginFade(false);
                    m_toCrouchingReferences[i].playerCanHide = false;
                }
            }
        }
    }
}