using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tutorial
{
    public class MoveToEffectTriggerHit : MonoBehaviour
    {
        private TutorialManager m_tutorialManager;

        private void Awake()
        {
            m_tutorialManager = transform.parent.GetComponent(typeof(TutorialManager)) as TutorialManager;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Player"))
            {
                m_tutorialManager.StepCompleted();
                transform.localPosition = new Vector3(-5.0f, -8f, 0.4f);
            }
        }
    }
}