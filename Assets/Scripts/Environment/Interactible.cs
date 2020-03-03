using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment.Highlighters;

namespace Environment
{
    public class Interactible : MonoBehaviour
    {
        [SerializeField]
        private Transform m_interactiblePosition;

        private bool m_isActive = false;

        public bool isActive
        {
            get { return m_isActive; }
        }

        public bool ActivateBehavior()
        {
            if (m_isActive)
            {
                return false;
            }
            m_isActive = true;
            return true;
        }

        public void DeactivateBehavior(bool permanent)
        {
            if (permanent)
            {
                this.enabled = false;
                MeshHighlighter meshHighlighter = GetComponent(typeof(MeshHighlighter)) as MeshHighlighter;
                if (meshHighlighter)
                {
                    meshHighlighter.enabled = false;
                }
                GroupMeshHighlighter groupMeshHighlighter = GetComponent(typeof(GroupMeshHighlighter)) as GroupMeshHighlighter;
                if (groupMeshHighlighter)
                {
                    groupMeshHighlighter.enabled = false;
                }
            }
            m_isActive = false;
        }

        public Vector3 GetInteractiblePosition()
        {
            return m_interactiblePosition.position;
        }
    }
}
