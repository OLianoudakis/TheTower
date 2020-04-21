using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.ChandalierEvent
{
    public class ChandalierDropEvent : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody m_chandalieRigidbody;

        public void ActivateDrop()
        {
            m_chandalieRigidbody.isKinematic = false;
            m_chandalieRigidbody.useGravity = true;
        }

        private IEnumerator RemoveChandalierDelay()
        {
            yield return new WaitForSeconds(5.0f);
            m_chandalieRigidbody.gameObject.SetActive(false);
        }
    }
}