using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.EmptyClass;

namespace AI.KnowledgeBase
{
    public class ShareKnowledge : MonoBehaviour
    {
        private KnowledgeBase m_knowledgeBase;

        private void Start()
        {
            m_knowledgeBase = GetComponent(typeof(KnowledgeBase)) as KnowledgeBase;
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyTagScript enemyTag = other.GetComponent(typeof(EnemyTagScript)) as EnemyTagScript;
            if (enemyTag)
            {
                KnowledgeBase kbOther = other.GetComponentInChildren(typeof(KnowledgeBase)) as KnowledgeBase;
                if (m_knowledgeBase.playerTransform && !kbOther.playerTransform)
                {
                    kbOther.playerTransform = m_knowledgeBase.playerTransform;
                }
                else if (m_knowledgeBase.playerHiding && !kbOther.playerTransform && !kbOther.playerHiding)
                {
                    kbOther.lastPlayerPosition = m_knowledgeBase.lastPlayerPosition;
                }
                if (m_knowledgeBase.noiseHeard && !kbOther.noiseHeard)
                {
                    kbOther.noisePosition = m_knowledgeBase.noisePosition;
                }
            }
        }
    }
}
