using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    public class Movable : MonoBehaviour
    {
        [SerializeField]
        private string m_name;

        [SerializeField]
        private float m_soundTransimissionTime = 0.5f;

        [SerializeField]
        private bool m_notFixable = false;

        [SerializeField]
        private float m_soundRadius = 10.0f;

        private Interactible m_interactible;
        private bool m_isMakingNoise = false;
        private float m_currentSoundTransimissionTime = 0.0f;
        private Rigidbody m_rigidBody;
        private Vector3 m_initialPosition;
        private Vector3 m_initialRotation;

        public string name
        {
            get { return m_name; }
        }

        public bool notFixable
        {
            get { return m_notFixable; }
        }

        public bool isMakingNoise
        {
            get { return m_isMakingNoise; }
        }

        public Vector3 initialPosition
        {
            get { return m_initialPosition; }
        }

        public Vector3 initialRotation
        {
            get { return m_initialRotation; }
        }

        public Transform movablePosition
        {
            get { return m_interactible.interactiblePosition; }
        }

        public bool CanMove(Transform transform)
        {
            return m_interactible.CanInteract(transform);
        }

        public bool HasTransformChanged()
        {
            return transform.hasChanged;
        }

        public void ResetChanges()
        {
            if (m_notFixable)
            {
                transform.hasChanged = false;
                return;
            }
            if (m_rigidBody)
            {
                m_rigidBody.isKinematic = true;
                m_rigidBody.useGravity = false;
            }
            transform.position = m_initialPosition;
            transform.rotation = Quaternion.Euler(m_initialRotation);
            transform.hasChanged = false;
        }

        private void Start()
        {
            m_rigidBody = GetComponent(typeof(Rigidbody)) as Rigidbody;
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
            if (!m_interactible)
            {
                m_interactible = GetComponentInParent(typeof(Interactible)) as Interactible;
            }
            transform.hasChanged = false;
            m_initialPosition = transform.position;
            m_initialRotation = transform.rotation.eulerAngles;
        }

        private void Update()
        {
            if (m_isMakingNoise)
            {
                m_currentSoundTransimissionTime += Time.deltaTime;
                if (m_currentSoundTransimissionTime >= m_soundTransimissionTime)
                {
                    m_currentSoundTransimissionTime = 0.0f;
                    m_isMakingNoise = false;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag.Equals("Noisable"))
            {
                Collider collider = GetComponent(typeof(Collider)) as Collider;
                CapsuleCollider capsuleCollider = collider as CapsuleCollider;
                if (capsuleCollider)
                {
                    m_rigidBody.isKinematic = true;
                    m_rigidBody.useGravity = false;
                    capsuleCollider.isTrigger = true;
                    capsuleCollider.radius *= m_soundRadius;
                }
                m_isMakingNoise = true;
            }
        }
    }
}
