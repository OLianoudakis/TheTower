using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.EmptyClass;

namespace AI.Perception
{
    public class SideSight : MonoBehaviour
    {
        private KnowledgeBase.KnowledgeBase m_knowledgeBase;

        private void Start()
        {
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerTagScript playerTag = other.GetComponent(typeof(PlayerTagScript)) as PlayerTagScript;
            if (playerTag)
            {
                m_knowledgeBase.PlayerSuspicion(other.transform.position);
            }
        }
    }
}
