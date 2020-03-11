using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Hiding
{
    public class HidespotShow : MonoBehaviour
    {
        public GameObject[] m_crouchIcons;

        private void OnMouseOver()
        {
            for (int i = 0; i < m_crouchIcons.Length; ++i)
            {
                m_crouchIcons[i].SetActive(true);
            }
        }

        private void OnMouseExit()
        {
            for (int i = 0; i < m_crouchIcons.Length; ++i)
            {
                m_crouchIcons[i].SetActive(false);
            }
        }

        private void Start()
        {
            for (int i = 0; i < m_crouchIcons.Length; ++i)
            {
                m_crouchIcons[i].SetActive(false);
            }
        }
    }
}