using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.KnowledgeBase;
using AI.KnowledgeBase.Parser;
using System;
using System.Linq;

namespace AI.Behavior.MotivationActions
{
    public class MotivationActionProperties : MonoBehaviour
    {
        [SerializeField]
        private MotivationGain m_motivationGain;

        [SerializeField]
        private KnowledgeBaseRuleset m_actionTriggerRules = KnowledgeBaseRulesEngine.GetKnowledgebaseRuleSet();

        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private Exp m_abstractSyntaxTree;

        public MotivationGain motivationGain
        {
            get { return m_motivationGain; }
        }

        public bool CanBeTriggered()
        {
            if (m_actionTriggerRules.knowledgeBaseRules.Length == 1)
            {
                if (!KnowledgeBaseRulesEngine.IsRuleTrue(
                    m_knowledgeBase, m_actionTriggerRules.knowledgeBaseRules[0].m_name, m_actionTriggerRules.knowledgeBaseRules[0].m_value))
                {
                    return false;
                }   
            }
            
            // TODO call abstract syntax tree interpreter
            return true;
        }

        private void Awake()
        {
            m_knowledgeBase = transform.parent.parent.GetComponentInChildren(typeof(KnowledgeBase.KnowledgeBase)) as KnowledgeBase.KnowledgeBase;
            LogicalOperationsParser parser
                = new LogicalOperationsParser(m_actionTriggerRules.logicalOperationSet.m_logicalOperationPrimitives, m_actionTriggerRules.knowledgeBaseRules);
            m_abstractSyntaxTree = parser.ParseRuleset();
            m_actionTriggerRules.knowledgeBaseRules = m_actionTriggerRules.knowledgeBaseRules.Where(val => val.m_useRule == true).ToArray();
            if ((m_actionTriggerRules.knowledgeBaseRules.Length > 1) && (m_abstractSyntaxTree == null))
            {
                throw new System.Exception("Logical operation set not provided for ruleset of length higher than 1!");
            }
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
