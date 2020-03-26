using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.EmptyClass;

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
            PlayerTagScript pt = other.gameObject.GetComponent(typeof(PlayerTagScript)) as PlayerTagScript;
            if (pt)
            {
                m_isActivated = true;
                this.gameObject.SetActive(false);
            }   
        }
    }
}
