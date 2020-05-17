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

        //For checkpoint reset
        private Vector3 m_initialTransformPosition;

        public bool gameOverAfterPlayerTouch
        {
            set { m_gameOverAfterPlayerTouch = value; }
            get { return m_gameOverAfterPlayerTouch; }
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void ResetPosition()
        {
            transform.position = m_initialTransformPosition;
            gameObject.SetActive(true);
        }

        private void Start()
        {
            Animator animator = GetComponentInChildren(typeof(Animator)) as Animator;
            animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
            m_light = GetComponentInChildren(typeof(Light)) as Light;
            m_initialTransformPosition = transform.position;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.parent && (collision.transform.parent.name.Equals("Floor")) && LayerMask.LayerToName(collision.transform.gameObject.layer).Contains("Default"))
            {
                m_light.cullingMask &= ~(1 << m_currentDefaultLayer);
                m_currentDefaultLayer = collision.transform.gameObject.layer;
                m_light.cullingMask |= (1 << m_currentDefaultLayer);
            }
        }
    }
}
