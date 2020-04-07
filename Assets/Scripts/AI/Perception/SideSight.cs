﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.EmptyClass;

namespace AI.Perception
{
    public class SideSight : MonoBehaviour
    {
        [SerializeField]
        private float m_sightYLocation = 2.0f;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private int m_layerMask;
        private CapsuleCollider m_collider;

        private void Start()
        {
            m_layerMask = LayerMask.GetMask("Default", "Hiding", "Player", "Highlight", "Walls");
            m_collider = transform.parent.parent.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerTagScript playerTag = other.GetComponent(typeof(PlayerTagScript)) as PlayerTagScript;
            if (playerTag)
            {
                Vector3 fromRay = new Vector3
                   (
                       m_collider.transform.position.x,
                       m_collider.transform.position.y + m_collider.center.y,
                       m_collider.transform.position.z
                   );
                Vector3 direction =
                    new Vector3(other.transform.position.x, other.transform.position.y + ((CapsuleCollider)other).center.y, other.transform.position.z)
                    - fromRay;
                RaycastHit hit;
                if (Physics.Raycast(fromRay, direction, out hit, Mathf.Infinity, m_layerMask)
                    && hit.collider.Equals(other))
                {
                    m_knowledgeBase.PlayerSuspicion(other.transform.position);
                }
            }
        }
    }
}
