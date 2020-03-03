using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.StateHandling.Idle
{
    public class Idle : MonoBehaviour
    {
        [SerializeField]
        private Animator m_animator;

        private void OnEnable()
        {
            m_animator.SetInteger("AnimState", 0);
        }

        private void OnDisable()
        {

        }

        private void Start()
        {
            m_animator.SetInteger("AnimState", 0);
        }
        private void Update()
        {

        }
    }
}
