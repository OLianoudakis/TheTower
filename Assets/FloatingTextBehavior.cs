using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextBehavior : MonoBehaviour
{
    private Camera m_cameraToLookAt;

    // Use this for initialization 
    private void Awake()
    {
        m_cameraToLookAt = Camera.main;
    }

    // Update is called once per frame 
    private void LateUpdate()
    {
        transform.LookAt(m_cameraToLookAt.transform);
        transform.rotation = Quaternion.LookRotation(m_cameraToLookAt.transform.forward);
    }
}
