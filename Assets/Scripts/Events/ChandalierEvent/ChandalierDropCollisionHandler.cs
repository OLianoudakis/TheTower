using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Events.ChandalierEvent
{
    public class ChandalierDropCollisionHandler : MonoBehaviour
    {
        private AudioSource m_audioSource;
        private bool m_firstCollision = true;

        private void Awake()
        {
            m_audioSource = GetComponent(typeof(AudioSource)) as AudioSource;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (m_firstCollision)
            {
                //TODO Add noise effect
                m_firstCollision = false;
                m_audioSource.Play();
            }
        }
    }
}