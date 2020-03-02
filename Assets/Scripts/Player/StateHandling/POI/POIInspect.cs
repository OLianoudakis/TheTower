using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace state
{
    namespace poiinspect
    {
        public class POIInspect : MonoBehaviour
        {
            [SerializeField]
            private Animator m_animator;

            [SerializeField]
            private POIDetector m_poiDetector;

            [SerializeField]
            private InputController m_inputController;

            private POIBehavior m_poiBehavior;
            private void OnEnable()
            {
                m_animator.SetInteger("AnimState", 0);
                m_poiBehavior = m_poiDetector.poiBehavior;
                m_poiBehavior.ActivatePOIBehavior();
                m_poiBehavior.ShowNextMessage();
            }

            private void OnDisable()
            {

            }

            private void Update()
            {
                if (m_poiBehavior && m_inputController.isLeftMouseClick)
                {
                    m_poiBehavior.ShowNextMessage();
                }
            }
        }
    }
}
