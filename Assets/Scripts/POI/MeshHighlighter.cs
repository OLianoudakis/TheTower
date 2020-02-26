using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshHighlighter : MonoBehaviour
{
    [SerializeField]
    private Material m_highlightMaterial;

    private Material[] m_originalMaterials;
    private MeshRenderer m_meshRenderer;

    private void Start()
    {
        m_meshRenderer = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
        if (m_meshRenderer)
        {
            m_originalMaterials = m_meshRenderer.materials;
        }
    }


    private void OnMouseEnter()
    {
        Material[] newMaterials = new Material[m_originalMaterials.Length];
        for (int i = 0; i < m_meshRenderer.materials.Length; i++)
        {
            m_highlightMaterial.SetTexture(Shader.PropertyToID("_TextureInput"), m_originalMaterials[i].GetTexture(Shader.PropertyToID("_BaseMap")));
            newMaterials[i] = m_highlightMaterial;
        }
        m_meshRenderer.materials = newMaterials;
    }

    private void OnMouseExit()
    {
        m_meshRenderer.materials = m_originalMaterials;
    }
}
