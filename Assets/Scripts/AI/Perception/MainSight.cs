﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.EmptyClass;

namespace AI.Perception
{
    public class MainSight : MonoBehaviour
    {
        private Transform m_playerTransform;
        private Transform m_myTransform;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;

        private void Start()
        {
            SharedAI sharedAI = FindObjectOfType(typeof(SharedAI)) as SharedAI;
            if (sharedAI)
            {
                m_playerTransform = sharedAI.playerTransform;
            }
            m_myTransform = GetComponent(typeof(Transform)) as Transform;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerTagScript playerTag = other.GetComponent(typeof(PlayerTagScript)) as PlayerTagScript;
            if (playerTag)
            {
                m_knowledgeBase.PlayerSpotted(m_playerTransform);
                return;
            }
            Movable movableObject = other.GetComponent(typeof(Movable)) as Movable;
            if (movableObject && movableObject.HasTransformChanged())
            {
                m_knowledgeBase.EnvironmentObjectMoved(movableObject);
            }
        }
    }
}
