using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Highlighters
{
    public class GroupMeshHighlighter : MonoBehaviour
    {
        [SerializeField]
        private Material m_highlightMaterial;

        private Material[][]  m_originalMaterials;
        private MeshRenderer[] m_meshRenderers;

        private void Start()
        {
            m_meshRenderers = GetComponentsInChildren<MeshRenderer>();
            m_originalMaterials = new Material[m_meshRenderers.Length][];
            for (int i = 0; i < m_meshRenderers.Length; i++)
            {
                m_originalMaterials[i] = new Material[m_meshRenderers[i].materials.Length];
                for (int j = 0; j < m_meshRenderers[i].materials.Length; j++)
                {
                    m_originalMaterials[i][j] = m_meshRenderers[i].materials[j];
                }
            }
        }


        private void OnMouseEnter()
        {
            if (this.enabled)
            {
                for (int i = 0; i < m_meshRenderers.Length; i++)
                {
                    Material[] newMaterials = new Material[m_originalMaterials[i].Length];
                    for (int j = 0; j < m_meshRenderers[i].materials.Length; j++)
                    {
                        m_highlightMaterial.SetTexture(Shader.PropertyToID("_TextureInput"), m_originalMaterials[i][j].GetTexture(Shader.PropertyToID("_BaseMap")));
                        newMaterials[j] = m_highlightMaterial;
                    }
                    m_meshRenderers[i].materials = newMaterials;
                }
            }
        }

        private void OnMouseExit()
        {
            for (int i = 0; i < m_meshRenderers.Length; i++)
            {
                m_meshRenderers[i].materials = m_originalMaterials[i];
            }
        }
    }
}
