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
        [SerializeField]
        private Transform[] m_playerSightPoints = null;

        private Transform m_playerTransform;
        private Transform m_myTransform;
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private int m_layerMask;
        private CapsuleCollider m_collider;
        private PlayerInvisibility m_playerInvisibility = null;

        private void Start()
        {
            m_layerMask = LayerMask.GetMask("Default", "Default2", "CrouchPosition", "Player", "Highlight", "Shadows");
            m_collider = transform.parent.parent.GetComponent(typeof(CapsuleCollider)) as CapsuleCollider;
            PlayerTagScript playerTag = FindObjectOfType(typeof(PlayerTagScript)) as PlayerTagScript;
            if (playerTag)
            {
                m_playerTransform = playerTag.transform;
            }
            m_myTransform = GetComponent(typeof(Transform)) as Transform;
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerInvisibility playerInvisibility = other.GetComponent(typeof(PlayerInvisibility)) as PlayerInvisibility;
            if (playerInvisibility)
            {
                m_playerInvisibility = playerInvisibility;
                if (!playerInvisibility.isInvisible)
                {
                    int hits = 0;
                    foreach (Transform raycastPoint in m_playerSightPoints)
                    {
                        RaycastHit hit;
                        if (Raycast(raycastPoint.position, out hit))
                        {
                            if (hit.collider.Equals(other))
                            {
                                ++hits;
                            }
                        }
                    }

                    if (hits > (m_playerSightPoints.Length / 2))
                    {
                        m_knowledgeBase.PlayerSpotted(m_playerTransform);
                    }
                    return;
                }
            }
            Movable movableObject = other.GetComponent(typeof(Movable)) as Movable;
            if (movableObject && movableObject.HasTransformChanged())
            {
                RaycastHit hit;
                if (Raycast(new Vector3(other.transform.position.x, other.transform.position.y, other.transform.position.z), out hit))
                {
                    // one movable hit
                    if (hit.collider.Equals(other))
                    {
                        // if it has parent
                        if(movableObject.transform.parent)
                        {
                            // add all objects at once
                            List<Movable> movedObjectsToUpdate = new List<Movable>();
                            Movable[] movedObjects = movableObject.transform.parent.GetComponentsInChildren<Movable>();
                            if (movedObjects != null)
                            {
                                foreach (Movable movableChild in movedObjects)
                                {
                                    if (movableChild.HasTransformChanged())
                                    {
                                        movedObjectsToUpdate.Add(movableChild);
                                    }
                                }
                                m_knowledgeBase.EnvironmentObjectsMoved(movedObjectsToUpdate);
                            }
                            return;
                        }
                        // else just add the object itself
                        m_knowledgeBase.EnvironmentObjectMoved(movableObject);
                        return;
                    }
                    // else if its object that posses movable children, send all of them at once (for the one level of children)
                    if (hit.collider.gameObject.Equals(other.transform.parent))
                    {
                        List<Movable> movedObjectsToUpdate = new List<Movable>();
                        Movable[] movables = hit.transform.GetComponentsInChildren<Movable>();
                        if (movables != null)
                        {
                            foreach (Movable movableChild in movables)
                            {
                                if (movableChild.HasTransformChanged())
                                {
                                    movedObjectsToUpdate.Add(movableChild);
                                }
                            }
                            m_knowledgeBase.EnvironmentObjectsMoved(movedObjectsToUpdate);
                        }
                        return;
                    }
                    // else try to check if its child of the hit object (any level)
                    Movable movable = hit.transform.GetComponentInChildren(typeof(Movable)) as Movable;
                    if (movable && (movable == movableObject))
                    {
                        m_knowledgeBase.EnvironmentObjectMoved(movableObject);
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
                int hits = 0;
                foreach (Transform raycastPoint in m_playerSightPoints)
                {
                    RaycastHit hit;
                    if (Raycast(raycastPoint.position, out hit))
                    {
                        if (hit.collider.gameObject.tag.Equals("Player"))
                        {
                            ++hits;
                        }
                    }
                }

                if (hits > (m_playerSightPoints.Length / 2))
                {
                    m_knowledgeBase.PlayerSpotted(m_playerTransform);
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
