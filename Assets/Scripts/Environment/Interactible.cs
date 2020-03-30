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

        private List<Transform> m_TransformsInDistance = new List<Transform>();
        private bool m_isActive = false;

        public Transform interactiblePosition
        {
            get { return m_interactiblePosition; }
            set { m_interactiblePosition = value; }
        }

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

        public bool CanInteract(Transform enemyTransform)
        {
            return m_TransformsInDistance.Find(x => x == enemyTransform);
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

        private void OnTriggerEnter(Collider other)
        {
            m_TransformsInDistance.Add(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            m_TransformsInDistance.Remove(other.transform);
        }
    }
}
