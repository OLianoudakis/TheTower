using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapOpaqueTransparent : MonoBehaviour
{
    [SerializeField]
    private GameObject m_opaqueObjectGroup;
    [SerializeField]
    private GameObject m_transparentObjectGroup;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            m_opaqueObjectGroup.SetActive(false);
            m_transparentObjectGroup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            m_opaqueObjectGroup.SetActive(true);
            m_transparentObjectGroup.SetActive(false);
        }
    }
}
