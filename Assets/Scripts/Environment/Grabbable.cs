using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Inventory;

namespace Environment
{
    public class Grabbable : MonoBehaviour
    {
        [SerializeField]
        private float m_heightOffset = 0.5f;

        private GrabbedObjectController m_grabbedObjectController;
        private Interactible m_interactible;
        private BoxCollider m_parentCollider;
        private BoxCollider m_boxCollider;
        private Vector3 m_parentOriginalPosition;
        private bool m_isActive = false;

        public void PlaceObject(Vector3 position)
        {
            m_parentCollider.enabled = true;
            m_boxCollider.enabled = true;
            transform.parent.position = new Vector3(position.x, position.y + m_heightOffset, position.z);
            transform.localPosition = Vector3.zero;
        }

        public void ResetToOriginalPosition()
        {
            m_parentCollider.enabled = true;
            m_boxCollider.enabled = true;
            transform.parent.position = m_parentOriginalPosition;
            transform.localPosition = Vector3.zero;
        }

        private void Awake()
        {
            m_grabbedObjectController = FindObjectOfType(typeof(GrabbedObjectController)) as GrabbedObjectController;
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
            m_parentCollider = transform.parent.GetComponent(typeof(BoxCollider)) as BoxCollider;
            m_boxCollider = GetComponent(typeof(BoxCollider)) as BoxCollider;
        }

        private void Start()
        {
            m_parentOriginalPosition = transform.parent.position;
        }

        private void Update()
        {
            if (m_interactible.isActive)
            {
                if (!m_isActive)
                {
                    m_isActive = true;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag.Equals("Player") && m_isActive)
            {
                if (!m_grabbedObjectController.isHoldingObject)
                {
                    m_interactible.DeactivateBehavior(false);
                    m_parentCollider.enabled = false;
                    m_boxCollider.enabled = false;
                    m_grabbedObjectController.GrabObject(transform.parent.gameObject);
                    m_isActive = false;
                }
            }
        }
    }
}