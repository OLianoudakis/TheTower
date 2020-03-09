using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Environment
{
    public class InteractibleTrigger : MonoBehaviour
    {
        [SerializeField]
        private Interactible m_interactible;
        private bool m_isActivated;

        public Interactible interactible
        {
            get { return m_interactible; }
        }

        public bool isActivated
        {
            get { return m_isActivated; }
            set { m_isActivated = value; }
        }

        private void OnTriggerEnter(Collider other)
        {
            // only player has input controller
            InputController ic = other.gameObject.GetComponent(typeof(InputController)) as InputController;
            if (ic)
            {
                m_isActivated = true;
                this.gameObject.SetActive(false);
            }   
        }
    }
}
