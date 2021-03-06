﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Player.StateHandling.Interact;

namespace Environment
{
    public class InteractibleDetector : MonoBehaviour
    {
        [SerializeField]
        private Transform m_playerPosition;

        private InputController m_inputController;
        private InteractibleTrigger[] m_interactibleTriggers;
        private Vector3 m_lastMousePosition = Vector3.zero;
        private Interactible m_interactible;

        public Interactible interactible
        {
            get { return m_interactible; }
        }

        private void Start()
        {
            m_inputController = FindObjectOfType(typeof(InputController)) as InputController;
            m_interactibleTriggers = FindObjectsOfType(typeof(InteractibleTrigger)) as InteractibleTrigger[];
        }

        private void Update()
        {
            // if interactible trigger was triggerred, take interactible and disable it
            foreach (InteractibleTrigger trigger in m_interactibleTriggers)
            {
                if (trigger.isActivated)
                {
                    m_interactible = trigger.interactible;
                    m_interactible.interactiblePosition = m_playerPosition;
                    trigger.isActivated = false;
                    return;
                }
            }
            // else check if interactible object was clicked
            if (m_inputController.isLeftMouseClick && (m_lastMousePosition != m_inputController.leftMouseClickPosition) && m_inputController.leftMouseClickHit.collider)
            {
                Interactible interactible = null;
                if (m_inputController.leftMouseClickHit.collider.gameObject.tag.Equals("Interactible"))
                {
                    interactible = m_inputController.leftMouseClickHit.collider.gameObject.GetComponent(typeof(Interactible)) as Interactible;
                    if (!interactible)
                    {
                        interactible = m_inputController.leftMouseClickHit.collider.gameObject.GetComponentInChildren(typeof(Interactible)) as Interactible;
                    }
                }
                if (m_interactible != interactible)
                {
                    if (m_interactible)
                    {
                        m_interactible.HighlightInteractible(false);
                    }
                    m_interactible = interactible;
                    // if interactible clicked and player is not in Interact state already
                    if (m_interactible && !(FindObjectOfType(typeof(Interact)) as Interact))
                    {
                        m_interactible.HighlightInteractible(true);
                    }
                }
            }
        }
    }
}
