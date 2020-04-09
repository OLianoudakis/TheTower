using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.EmptyClass
{
    public class EnemyTagScript : MonoBehaviour
    {
        [SerializeField]
        private bool m_gameOverAfterPlayerTouch = true;

        private Animator m_animator;

        public bool gameOverAfterPlayerTouch
        {
            get { return m_gameOverAfterPlayerTouch; }
        }

        private void Start()
        {
            m_animator = GetComponentInChildren(typeof(Animator)) as Animator;
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
        }
    }
}
