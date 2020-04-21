using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles.GargoylePuzze
{
    public class EyePuzzleActivate : MonoBehaviour
    {
        [SerializeField]
        private EyePuzzleController m_eyePuzzleController;

        public void ActivateEye()
        {
            m_eyePuzzleController.ActivateEye();
        }
    }
}