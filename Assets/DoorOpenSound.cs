using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenSound : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_doorOpeningSFX;

    public void OpenDoorSFX()
    {
        (GetComponent(typeof(AudioSource)) as AudioSource).PlayOneShot(m_doorOpeningSFX);
    }
}
