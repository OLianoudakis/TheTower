using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class ActivateObjectOnTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_objectToBeActivated;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_objectToBeActivated.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_objectToBeActivated.SetActive(false);
            }
        }
    }
}