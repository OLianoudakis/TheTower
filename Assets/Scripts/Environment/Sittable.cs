using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.EmptyClass;

namespace Environment
{
    public class Sittable : MonoBehaviour
    {
        private Interactible m_interactible;

        public bool CanSit(Transform enemyTransform)
        {
            return m_interactible.CanInteract(enemyTransform);
        }

        private void Start()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
        }
    }
}
