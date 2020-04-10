using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerSFXAudioController : MonoBehaviour
    {
        [SerializeField]
        private AudioSource m_footstepSource;
        [SerializeField]
        private AudioSource m_inventorySource;
        [SerializeField]
        private AudioClip m_footstepClip;
        [SerializeField]
        private AudioClip m_keyAddedClip;
        [SerializeField]
        private AudioClip m_unlockDoorClip;

        public void PlayFootstep()
        {
            m_footstepSource.PlayOneShot(m_footstepClip);
        }

        public void PlayKeySound()
        {
            m_inventorySource.PlayOneShot(m_keyAddedClip);
        }

        public void PlayUnlockSound()
        {
            m_inventorySource.PlayOneShot(m_unlockDoorClip);
        }
    }
}