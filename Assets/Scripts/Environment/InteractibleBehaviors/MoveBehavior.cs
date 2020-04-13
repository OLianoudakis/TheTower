using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment.InteractibleBehaviors
{
    public class MoveBehavior : MonoBehaviour
    {
        [SerializeField]
        private bool m_rotateOnly = false;

        [SerializeField]
        private bool m_rotateAroundX = false;

        [SerializeField]
        private bool m_rotateAroundY = false;

        [SerializeField]
        private bool m_rotateAroundZ = false;

        private Interactible m_interactible;
        private bool m_used = false;

        private void Awake()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
        }

        private void Update()
        {
            if (m_interactible.isActive)
            {
                foreach (Transform child in transform)
                {
                    if (child.GetComponent(typeof(Movable)) as Movable)
                    {
                        m_used = true;
                        if (m_rotateOnly)
                        {
                            float x = 0.0f;
                            float y = 0.0f;
                            float z = 0.0f;
                            Vector3 rotation = Random.rotation.eulerAngles;
                            if (m_rotateAroundX)
                            {
                                x = rotation.x;
                            }
                            if (m_rotateAroundY)
                            {
                                y = rotation.y;
                            }
                            if (m_rotateAroundZ)
                            {
                                z = rotation.z;
                            }
                            transform.Rotate(new Vector3(x, y, z));
                        }
                        else
                        {
                            Rigidbody rigidbody = child.GetComponent(typeof(Rigidbody)) as Rigidbody;
                            if (rigidbody)
                            {
                                rigidbody.isKinematic = false;
                                rigidbody.useGravity = true;
                                rigidbody.AddForce(new Vector3(0.5f, 0.5f, 0.5f) * 100.0f);
                            }
                        }
                    }
                }
                m_interactible.DeactivateBehavior(true);
            }
            else if (m_used)
            {
                foreach (Transform child in transform)
                {
                    Movable movable = child.GetComponent(typeof(Movable)) as Movable;
                    if (movable && movable.HasTransformChanged())
                    {
                        return;
                    }
                }
                m_interactible.RestartBehavior();
                m_used = false;
            }
        }
    }
}
