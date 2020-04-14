using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class InputController : MonoBehaviour
    {
        private Vector3 m_leftMouseClickPosition = Vector3.zero;

        private bool m_initialized = false;
        private RaycastHit m_leftMouseClickHit;
        private bool m_isLeftMouseClick = false;
        private int m_layerMask;

        public bool initialized
        {
            get { return m_initialized; }
        }

        public bool isLeftMouseClick
        {
            get { return m_isLeftMouseClick; }
        }

        public Vector3 leftMouseClickPosition
        {
            get { return m_leftMouseClickPosition; }
            set { m_leftMouseClickPosition = value; } // we want to be able to adjust this because of navmesh imperfections
        }

        public RaycastHit leftMouseClickHit
        {
            get { return m_leftMouseClickHit; }
        }

        private void Update()
        {
            if (!m_initialized)
            {
                m_initialized = true;
                m_leftMouseClickPosition = transform.position;
            }
            if (Input.GetMouseButtonDown(0))
            {
                m_isLeftMouseClick = true;
                RaycastHit hit;
                if (Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, m_layerMask))
                {
                    Debug.Log(hit.transform.gameObject);
                    // ignore walls
                    if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Walls"))
                    {
                        // if interactible object hit, accept
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Highlight"))
                        {
                            m_leftMouseClickPosition = hit.point;
                            m_leftMouseClickHit = hit;
                            return;
                        }
                        // else check if on navmesh and adjust
                        NavMeshHit navhit;
                        if (NavMesh.FindClosestEdge(hit.point, out navhit, NavMesh.AllAreas))
                        {
                            Vector3 position = hit.point;
                            if (float.IsInfinity(navhit.distance) || (navhit.distance < 0.05f))
                            {
                                position = navhit.position;
                            }
                            m_leftMouseClickPosition = position;
                            m_leftMouseClickHit = hit;
                            return;
                        }
                    }
                }
            }
            m_isLeftMouseClick = false;
        }

        private void Start()
        {
            m_layerMask = LayerMask.GetMask("Default", "CrouchPosition", "Highlight", "Walls");
        }
    }
}
