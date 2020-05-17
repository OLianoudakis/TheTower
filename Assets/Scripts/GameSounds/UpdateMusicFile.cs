using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSounds;

public class UpdateMusicFile : MonoBehaviour
{
    [SerializeField]
    private AudioClip m_levelMusic;

    private MusicTagScript m_musicObject;

    private void Awake()
    {
        m_musicObject = FindObjectOfType(typeof(MusicTagScript)) as MusicTagScript;
    }
    private void Start()
    {
        (m_musicObject.GetComponent(typeof(AudioSource)) as AudioSource).Stop();
        (m_musicObject.GetComponent(typeof(AudioSource)) as AudioSource).clip = m_levelMusic;
        (m_musicObject.GetComponent(typeof(AudioSource)) as AudioSource).Play();
    }
}
