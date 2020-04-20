using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.EmptyClass;
using Player;

namespace AI.Perception
{
    public class SideSight : MonoBehaviour
    {
        [SerializeField]
        private float m_sightYLocation = 2.0f;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private int m_layerMask;
        private CapsuleCollider m_collider;
        private float m_playerColliderCenter;
        private PlayerInvisibility m_playerInvisibility = null;


        private void Start()
        {
            m_layerMask = LayerMask.GetMask("Default", "Default2", "CrouchPosition", "Player", "Highlight", "Shadows");
            m_collider = transform.parent.parent.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerInvisibility playerInvisibility = other.GetComponent(typeof(PlayerInvisibility)) as PlayerInvisibility;
            if (playerInvisibility)
            {
                m_playerColliderCenter = (other.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider).center.y;
                m_playerInvisibility = playerInvisibility;
                if (!playerInvisibility.isInvisible)
                {
                    RaycastHit hit;
                    if (Raycast(new Vector3(other.transform.position.x, other.transform.position.y + ((CapsuleCollider)other).center.y, other.transform.position.z), out hit))
                    {
                        if (hit.collider.Equals(other))
                        {
                            m_knowledgeBase.PlayerSuspicion(other.transform.position);
                        }
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerInvisibility playerInvisibility = other.GetComponent(typeof(PlayerInvisibility)) as PlayerInvisibility;
            if (playerInvisibility)
            {
                m_playerInvisibility = null;
            }
        }

        private void Update()
        {
            if (m_playerInvisibility && !m_playerInvisibility.isInvisible)
            {
                RaycastHit hit;
                if (Raycast(new Vector3(m_playerInvisibility.transform.position.x, m_playerInvisibility.transform.position.y + m_playerColliderCenter, m_playerInvisibility.transform.position.z), out hit))
                {
                    if (hit.collider.gameObject.tag.Equals("Player"))
                    {
                        m_knowledgeBase.PlayerSuspicion(m_playerInvisibility.transform.position);
                    }
                }
            }
        }

        private bool Raycast(Vector3 otherPosition, out RaycastHit hit)
        {
            // * 2.0f to go from eyes
            Vector3 fromRay = new Vector3
            (
                m_collider.transform.position.x,
                m_collider.transform.position.y + (2.0f * m_collider.center.y),
                m_collider.transform.position.z
            );
            Vector3 direction = otherPosition - fromRay;

            if (Physics.Raycast(fromRay, direction, out hit, Mathf.Infinity, m_layerMask))
            {
                return true;
            }
            return false;
        }
    }
}
