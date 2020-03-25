using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.EmptyClass
{
    public class EnemyTagScript : MonoBehaviour
    {
        private Animator m_animator;

        private void Start()
        {
            m_animator = GetComponentInChildren(typeof(Animator)) as Animator;
            m_animator.SetInteger(AnimationConstants.ButtlerAnimationState, AnimationConstants.AnimButtlerIdle);
        }
    }
}
