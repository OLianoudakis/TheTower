using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class EnemySFXAudioController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource m_footstepSource;

        [SerializeField]
        private AudioClip m_footstepClip;

        public void PlayFootstep()
        {
            m_footstepSource.PlayOneShot(m_footstepClip);
        }
    }
}