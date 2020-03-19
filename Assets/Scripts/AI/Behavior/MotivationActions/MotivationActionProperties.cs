using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.KnowledgeBase;
using System;
using System.Linq;

namespace AI.Behavior.MotivationActions
{
    public class MotivationActionProperties : MonoBehaviour
    {
        [SerializeField]
        private MotivationGain m_motivationGain;

        [SerializeField]
        private KnowledgeBaseRule[] m_actionTriggerRules = KnowledgeBaseRulesEngine.GetKnowledgebaseRuleSet();

        private KnowledgeBase.KnowledgeBase m_knowledgeBase;

        public MotivationGain motivationGain
        {
            get { return m_motivationGain; }
        }

        public bool CanBeTriggered()
        {
            foreach (KnowledgeBaseRule actionTriggerRule in m_actionTriggerRules)
            {
                if (!KnowledgeBaseRulesEngine.IsRuleTrue(m_knowledgeBase, actionTriggerRule.m_name, actionTriggerRule.m_value))
                {
                    return false;
                }
            }
            return true;
        }

        private void Awake()
        {
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
            m_actionTriggerRules = m_actionTriggerRules.Where(val => val.m_useRule == true).ToArray();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
