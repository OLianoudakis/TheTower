using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.EmptyClass
{
    public class EnemyTagScript : MonoBehaviour
    {
        [SerializeField]
        private bool m_gameOverAfterPlayerTouch = true;

        private Light m_light;
        private int m_currentDefaultLayer = 0;

        public bool gameOverAfterPlayerTouch
        {
            get { return m_gameOverAfterPlayerTouch; }
        }

        private void Start()
        {
            Animator animator = GetComponentInChildren(typeof(Animator)) as Animator;
            animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            m_light = GetComponentInChildren(typeof(Light)) as Light;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.parent && (other.transform.parent.name.Equals("Floor")) && LayerMask.LayerToName(other.transform.gameObject.layer).Contains("Default"))
            {
                m_light.cullingMask &= ~(1 << m_currentDefaultLayer);
                m_currentDefaultLayer = other.transform.gameObject.layer;
                m_light.cullingMask |= (1 << m_currentDefaultLayer);
            }
        }
    }
}
