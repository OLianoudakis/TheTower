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

        private void Start()
        {
            m_layerMask = LayerMask.GetMask("Default", "Default2", "CrouchPosition", "Player", "Highlight", "Shadows");
            m_collider = transform.parent.parent.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            CheckVisibility(other);
        }

        private void OnTriggerStay(Collider other)
        {
            CheckVisibility(other);
        }

        private void CheckVisibility(Collider other)
        {
            PlayerTagScript playerTag = other.GetComponent(typeof(PlayerTagScript)) as PlayerTagScript;
            PlayerInvisibility playerInvisibility = other.GetComponent(typeof(PlayerInvisibility)) as PlayerInvisibility;
            if (playerTag && playerInvisibility && !playerInvisibility.isInvisible)
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
