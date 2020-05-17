using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class PlayerTutorialManager : MonoBehaviour
    {
        private Animator m_animator;

        public void StandUpOver()
        {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            m_animator.applyRootMotion = false;
        }

        private void Awake()
        {
            m_animator = GetComponentInChildren(typeof(Animator)) as Animator;
        }
    }
}