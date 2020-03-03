using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.InteractibleBehaviors
{
    public class MessBehavior : MonoBehaviour
    {
        private Interactible m_interactible;

        private void Start()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
        }

        private void Update()
        {
            if (m_interactible.isActive)
            {
                foreach (Transform child in transform)
                {
                    Rigidbody rigidbody = child.GetComponent(typeof(Rigidbody)) as Rigidbody;
                    if (rigidbody)
                    {
                        rigidbody.AddForce(Random.onUnitSphere);
                    }
                }
                m_interactible.DeactivateBehavior(true);
                this.enabled = false;
            }
        }
    }
}
