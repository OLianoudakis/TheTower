using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.StateHandling
{
    public class GetActiveState : MonoBehaviour
    {
        private GameObject m_activeChild;

        public GameObject GetActivePlayerState()
        {
            return m_activeChild;
        }

        // Update is called once per frame
        void Update()
        {
            bool foundOne = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.active)
                {
                    m_activeChild = transform.GetChild(i).gameObject;
                    foundOne = true;
                    break;
                }
            }
            if (!foundOne)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }
}