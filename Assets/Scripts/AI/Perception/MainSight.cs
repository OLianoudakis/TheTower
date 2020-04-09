using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.EmptyClass;
using Environment;
using Player;

namespace AI.Perception
{
    public class MainSight : MonoBehaviour
    {
        private Transform m_playerTransform;
        private Transform m_myTransform;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private int m_layerMask;
        private CapsuleCollider m_collider;

        private void Start()
        {
            m_layerMask = LayerMask.GetMask("Default", "Hiding", "Player", "Highlight", "Walls", "Furniture", "Shadows");
            m_collider = transform.parent.parent.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
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
            PlayerInvisibility playerInvisibility = other.GetComponent(typeof(PlayerInvisibility)) as PlayerInvisibility;
            if (playerTag && playerInvisibility && !playerInvisibility.isInvisible)
            {
                RaycastHit hit;
                if (Raycast(new Vector3(other.transform.position.x, other.transform.position.y + ((CapsuleCollider)other).center.y, other.transform.position.z), out hit))
                {
                    if (hit.collider.Equals(other))
                    {
                        m_knowledgeBase.PlayerSpotted(m_playerTransform);
                        return;
                    }
                }
            }
            Movable movableObject = other.GetComponent(typeof(Movable)) as Movable;
            if (movableObject && movableObject.HasTransformChanged())
            {
                RaycastHit hit;
                if (Raycast(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), out hit))
                {
                    if (hit.collider.Equals(other))
                    {
                        m_knowledgeBase.EnvironmentObjectMoved(movableObject);
                        return;
                    }
                    // else try to check if its child of the hit object
                    Movable movable = hit.transform.GetComponentInChildren(typeof(Movable)) as Movable;
                    if (movable == movableObject)
                    {
                        m_knowledgeBase.EnvironmentObjectMoved(movableObject);
                    }
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            PlayerTagScript playerTag = other.GetComponent(typeof(PlayerTagScript)) as PlayerTagScript;
            if (playerTag)
            {
                RaycastHit hit;
                if (Raycast(new Vector3(other.transform.position.x, other.transform.position.y + ((CapsuleCollider)other).center.y, other.transform.position.z), out hit))
                {
                    if (hit.collider.Equals(other))
                    {
                        m_knowledgeBase.PlayerSpotted(m_playerTransform);
                    }
                }
            }
        }

        private bool Raycast(Vector3 otherPosition, out RaycastHit hit)
        {
            Vector3 fromRay = new Vector3
            (
                m_collider.transform.position.x,
                m_collider.transform.position.y + m_collider.center.y,
                m_collider.transform.position.z
            );
            Vector3 direction =
                    new Vector3(otherPosition.x, otherPosition.y, otherPosition.z)
                    - fromRay;

            if (Physics.Raycast(fromRay, direction, out hit, Mathf.Infinity, m_layerMask))
            {
                return true;
            }
            return false;
        }
    }
}
