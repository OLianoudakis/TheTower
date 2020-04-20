using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.EmptyClass;
using Environment;

namespace AI.Perception
{
    public class Hearing : MonoBehaviour
    {
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private List<Movable> m_movablesWithinBounds = new List<Movable>();

        private void Start()
        {
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            Movable movableObject = other.GetComponent(typeof(Movable)) as Movable;
            if (movableObject)
            {
                m_movablesWithinBounds.Add(movableObject);
                if (movableObject.isMakingNoise)
                {
                    m_knowledgeBase.NoiseHeard(other.transform.position);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Movable movableObject = other.GetComponent(typeof(Movable)) as Movable;
            if (movableObject)
            {
                m_movablesWithinBounds.Remove(movableObject);
            }
        }

        private void Update()
        {
            foreach (Movable movableObject in m_movablesWithinBounds)
            {
                if (movableObject.isMakingNoise)
                {
                    m_knowledgeBase.NoiseHeard(movableObject.transform.position);
                }
            }
        }
    }
}
