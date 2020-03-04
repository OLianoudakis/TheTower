﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Player.StateHandling.POI
{
    public class ToIdle : MonoBehaviour
    {
        [SerializeField]
        private int m_priority;

        [SerializeField]
        private InteractibleDetector m_interactibleDetector;

        [SerializeField]
        private GameObject m_idleState;

        private TransitionHandler m_transitionHandler;
        private Interactible m_interactible;

        private void OnEnable()
        {
            m_interactible = m_interactibleDetector.interactible;
        }

        private void Start()
        {
            m_transitionHandler = GetComponent(typeof(TransitionHandler)) as TransitionHandler;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!m_interactible.enabled || !m_interactible.isActive)
            {
                m_transitionHandler.AddActiveTransition(m_priority, m_idleState);
            }
        }
    }
}