using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.Hiding
{
    public class HidespotShow : MonoBehaviour
    {
        public GameObject[] m_crouchIcons;
        private bool m_permanentShow = false;

        public bool permanentShow
        {
            set { m_permanentShow = value; }
        }

        public void ShowIcon(bool showIcon)
        {
            if (showIcon && !m_permanentShow)
            {
                for (int i = 0; i < m_crouchIcons.Length; ++i)
                {
                    m_crouchIcons[i].SetActive(true);
                }
            }
            else if (!m_permanentShow)
            {
                for (int i = 0; i < m_crouchIcons.Length; ++i)
                {
                    m_crouchIcons[i].SetActive(false);
                }
            }
        }

        private void OnMouseOver()
        {
            ShowIcon(true);
        }

        private void OnMouseExit()
        {
            ShowIcon(false);
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