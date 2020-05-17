using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles.GargoylePuzze
{
    public class EyePuzzleActivate : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_particleEffect;
        [SerializeField]
        private EyePuzzleController m_eyePuzzleController;

        public void ActivateEye()
        {
            m_particleEffect.SetActive(false);
            m_eyePuzzleController.ActivateEye();
        }
    }
}