using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField]
    private string m_name;

    [SerializeField]
    private float m_soundTransimissionTime = 0.5f;

    private bool m_isMakingNoise = false;
    private float m_currentSoundTransimissionTime = 0.0f;
    private Rigidbody m_rigidBody;
    private Vector3 m_initialPosition;
    private Quaternion m_initialRotation;

    public string name
    {
        get { return m_name;}
    }

    public bool isMakingNoise
    {
        get { return m_isMakingNoise; }
    }

    public Vector3 initialPosition
    {
        get { return m_initialPosition; }
    }

    public Quaternion initialRotation
    {
        get { return m_initialRotation; }
    }

    public bool HasTransformChanged()
    {
        bool hasChanged = transform.hasChanged;
        transform.hasChanged = false;
        return hasChanged;
    }

    public void ResetChanges()
    {
        transform.hasChanged = false;
    }

    public void ResetProperties()
    {
        m_rigidBody.isKinematic = true;
        m_rigidBody.useGravity = false;
    }

    private void Start()
    {
        m_rigidBody = GetComponent(typeof(Rigidbody)) as Rigidbody;
        transform.hasChanged = false;
    }

    private void Update()
    {
        if (m_isMakingNoise)
        {
            m_currentSoundTransimissionTime += Time.deltaTime;
            if (m_currentSoundTransimissionTime >= m_soundTransimissionTime)
            {
                m_isMakingNoise = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Noisable"))
        {
            m_isMakingNoise = true;
        }
    }
}
