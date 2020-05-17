using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Environment;
using Environment.InteractibleBehaviors;
using Environment.Highlighters;

namespace Puzzles.GargoylePuzze
{
    public class EyePuzzleController : MonoBehaviour
    {
        [SerializeField]
        private POIBehavior m_poiBeforeSolve;
        [SerializeField]
        private POIBehavior m_poiAfterSolve;
        [SerializeField]
        private GameObject m_leftEye;
        [SerializeField]
        private GameObject m_rightEye;
        [SerializeField]
        private Interactible m_interactible;
        [SerializeField]
        private MeshHighlighter m_meshHighlighter;

        private int m_activeEyes = 0;

        public void ActivateEye()
        {
            m_activeEyes++;
            if (m_activeEyes == 1)
            {
                m_rightEye.SetActive(true);
            }
            else if (m_activeEyes == 2)
            {
                m_leftEye.SetActive(true);
                SwitchPOI();
            }

        }

        private void SwitchPOI()
        {
            m_poiBeforeSolve.enabled = false;
            m_poiAfterSolve.enabled = true;
            m_interactible.enabled = true;
            m_meshHighlighter.enabled = true;
        }
    }
}