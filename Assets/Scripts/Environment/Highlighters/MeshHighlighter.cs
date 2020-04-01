using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Highlighters
{
    public class MeshHighlighter : MonoBehaviour
    {
        [SerializeField]
        private Material m_highlightMaterial;

        private Material[] m_originalMaterials;
        private MeshRenderer m_meshRenderer;
        private int m_layerMask;
        private Collider m_collider;

        private void Start()
        {
            m_meshRenderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
            m_collider = GetComponent(typeof(Collider)) as Collider;
            if (m_meshRenderer)
            {
                m_originalMaterials = m_meshRenderer.materials;
            }
            else
            {
                m_meshRenderer = GetComponentInChildren(typeof(MeshRenderer)) as MeshRenderer;
                if (m_meshRenderer)
                {
                    m_originalMaterials = m_meshRenderer.materials;
                }
            }
            m_layerMask = LayerMask.GetMask("Highlight");
        }

        private void Update()
        {
            if (this.enabled)
            {
                RaycastHit hit;
                if ((Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, m_layerMask))
                    && hit.collider == m_collider)
                {
                    
                    Material[] newMaterials = new Material[m_originalMaterials.Length];
                    for (int i = 0; i < m_meshRenderer.materials.Length; i++)
                    {
                        m_highlightMaterial.SetTexture(Shader.PropertyToID("_TextureInput"), m_originalMaterials[i].GetTexture(Shader.PropertyToID("_BaseMap")));
                        newMaterials[i] = m_highlightMaterial;
                    }
                    m_meshRenderer.materials = newMaterials;
                }
                else
                {
                    m_meshRenderer.materials = m_originalMaterials;
                }
            }
        }
    }
}
