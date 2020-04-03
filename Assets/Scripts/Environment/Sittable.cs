﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.EmptyClass;

namespace Environment
{
    public class Sittable : MonoBehaviour
    {
        [SerializeField]
        private string m_name;

        private Interactible m_interactible;

        public string name
        {
            get { return m_name; }
        }

        public Transform sittablePosition
        {
            get { return m_interactible.interactiblePosition; }
        }

        public bool CanSit(Transform transform)
        {
            return m_interactible.CanInteract(transform);
        }

        private void Start()
        {
            m_interactible = GetComponent(typeof(Interactible)) as Interactible;
        }
    }
}
