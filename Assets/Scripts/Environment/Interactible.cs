using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment.Highlighters;
using Environment.Hiding;

namespace Environment
{
    public class Interactible : MonoBehaviour
    {
        [SerializeField]
        private Transform m_interactiblePosition;

        private List<Transform> m_TransformsInDistance = new List<Transform>();
        private MeshHighlighter m_meshHighlighter;
        private GroupMeshHighlighter m_groupMeshHighlighter;
        private HidespotShow m_hidespotHighlight;
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
                if (m_meshHighlighter)
                {
                    m_meshHighlighter.permanentHighlight = false;
                    m_meshHighlighter.HighlightMesh(false);
                    m_meshHighlighter.enabled = false;
                }
                if (m_groupMeshHighlighter)
                {
                    m_groupMeshHighlighter.permanentHighlight = false;
                    m_groupMeshHighlighter.HighlightMesh(false);
                    m_groupMeshHighlighter.enabled = false;
                }
            }
            m_isActive = false;
        }

        public void HighlightInteractible(bool highlightInteractible)
        {
            if (m_meshHighlighter && m_meshHighlighter.enabled)
            {
                if (highlightInteractible)
                {
                    m_meshHighlighter.HighlightMesh(highlightInteractible);
                    m_meshHighlighter.permanentHighlight = true;
                }
                else
                {
                    m_meshHighlighter.permanentHighlight = false;
                    m_meshHighlighter.HighlightMesh(highlightInteractible);
                }
            }
            if (m_groupMeshHighlighter && m_groupMeshHighlighter.enabled)
            {
                if (highlightInteractible)
                {
                    m_groupMeshHighlighter.HighlightMesh(highlightInteractible);
                    m_groupMeshHighlighter.permanentHighlight = true;
                }
                else
                {
                    m_groupMeshHighlighter.permanentHighlight = false;
                    m_groupMeshHighlighter.HighlightMesh(highlightInteractible);
                }
            }
            if (m_hidespotHighlight)
            {
                if (highlightInteractible)
                {
                    m_hidespotHighlight.ShowIcon(highlightInteractible);
                    m_hidespotHighlight.permanentShow = true;
                }
                else
                {
                    m_hidespotHighlight.permanentShow = false;
                    m_hidespotHighlight.ShowIcon(highlightInteractible);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            m_TransformsInDistance.Add(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            m_TransformsInDistance.Remove(other.transform);
        }

        private void Start()
        {
            m_meshHighlighter = GetComponent(typeof(MeshHighlighter)) as MeshHighlighter;
            if (!m_meshHighlighter)
            {
                m_meshHighlighter = GetComponentInParent(typeof(MeshHighlighter)) as MeshHighlighter;
            }
            m_groupMeshHighlighter = GetComponent(typeof(GroupMeshHighlighter)) as GroupMeshHighlighter;
            if (!m_groupMeshHighlighter)
            {
                m_groupMeshHighlighter = GetComponentInParent(typeof(GroupMeshHighlighter)) as GroupMeshHighlighter;
            }
            m_hidespotHighlight = GetComponent(typeof(HidespotShow)) as HidespotShow;
        }
    }
}
