using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;

namespace Player.StateHandling.POI
{
    public class Interact : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        [SerializeField]
        private InteractibleDetector m_interactibleDetector;
        
        private void OnEnable()
        {
            m_animator.SetInteger("AnimState", 0);
            if (m_interactibleDetector.interactible)
            {
                m_interactibleDetector.interactible.ActivateBehavior();
            }
        }
    }
}
