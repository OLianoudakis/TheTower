using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Player;
using Environment;

namespace Player.Inventory
{
    public class GrabbedObjectController : MonoBehaviour
    {
        public bool isHoldingObject
        {
            get { return m_hodlingObject; }
        }

        [SerializeField]
        private LayerMask m_layerMask;
        [SerializeField]
        private Material m_canPlaceMaterial;
        [SerializeField]
        private Material m_cannotPlaceMaterial;
        private Material m_previousMaterial;
        private MeshRenderer m_grabbedObjectRenderer;
        private GameObject m_grabbedObject;
        private Transform m_grabbedObjectPreviousParent;
        private bool m_hodlingObject = false;
        private float m_onMeshThreshold = 3.0f;
        private int m_grabbedObjectOriginalLayer;
        private InputController m_inputController;

        public void GrabObject(GameObject grabbedObject)
        {
            if (m_hodlingObject)
            {
                return;
            }
            m_grabbedObject = grabbedObject;
            m_grabbedObjectPreviousParent = m_grabbedObject.transform.parent;
            Debug.Log("PARENT NAME: " + m_grabbedObjectPreviousParent.gameObject.name);
            m_grabbedObjectRenderer = m_grabbedObject.GetComponentInChildren(typeof(MeshRenderer)) as MeshRenderer;
            m_previousMaterial = m_grabbedObjectRenderer.material;
            m_grabbedObject.transform.parent = transform;
            m_grabbedObject.transform.localPosition = Vector3.zero;
            m_grabbedObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            m_grabbedObjectOriginalLayer = m_grabbedObject.layer;
            m_grabbedObject.layer = 2;
            m_hodlingObject = true;
        }

        public void ResetGrabbedObjectToOrigin()
        {
            if (m_hodlingObject)
            {
                m_hodlingObject = false;
                m_grabbedObject.layer = m_grabbedObjectOriginalLayer;
                m_grabbedObject.transform.parent = m_grabbedObjectPreviousParent;
                m_grabbedObject.GetComponentInChildren<Grabbable>().ResetToOriginalPosition();
                m_grabbedObject = null;
            }
        }

        private void Awake()
        {
            m_inputController = transform.parent.GetComponent(typeof(InputController)) as InputController;
        }

        private void Update()
        {
            if (!m_hodlingObject)
            {
                return;
            }

            bool canPlaceItem = false;
            bool rayHittingGround = false;
            Vector3 rayHitPointPosition = Vector3.zero;
            if (m_hodlingObject)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(transform.position, Vector3.down, out raycastHit, 10.0f, m_layerMask))
                {
                    if (raycastHit.collider.gameObject.tag.Equals("Ground"))
                    {
                        rayHittingGround = true;
                        rayHitPointPosition = raycastHit.point;
                    }
                }
            }

            if (rayHittingGround)
            {
                NavMeshHit navMeshHit;
                if (NavMesh.SamplePosition(rayHitPointPosition, out navMeshHit, m_onMeshThreshold, NavMesh.AllAreas))
                {
                    if (Mathf.Approximately(rayHitPointPosition.x, navMeshHit.position.x) 
                        && Mathf.Approximately(rayHitPointPosition.z, navMeshHit.position.z))
                    {
                        canPlaceItem = true;
                    }
                }
            }

            if (canPlaceItem)
            {
                m_grabbedObjectRenderer.material = m_canPlaceMaterial;
                if (m_inputController.isPlaceItemKeyDown && m_grabbedObject)
                {
                    m_hodlingObject = false;
                    m_grabbedObject.layer = m_grabbedObjectOriginalLayer;
                    m_grabbedObject.transform.parent = m_grabbedObjectPreviousParent;
                    m_grabbedObject.GetComponentInChildren<Grabbable>().PlaceObject(rayHitPointPosition);
                    m_grabbedObject = null;
                }
            }
            else
            {
                m_grabbedObjectRenderer.material = m_cannotPlaceMaterial;
            }
        }
    }
}