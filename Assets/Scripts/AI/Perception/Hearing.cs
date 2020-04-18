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

        private void Start()
        {
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            Movable movableObject = other.GetComponent(typeof(Movable)) as Movable;
            if (movableObject && movableObject.isMakingNoise)
            {
                m_knowledgeBase.NoiseHeard(other.transform.position);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Movable movableObject = other.GetComponent(typeof(Movable)) as Movable;
            if (movableObject && movableObject.isMakingNoise)
            {
                m_knowledgeBase.NoiseHeard(other.transform.position);
            }
        }
    }
}
