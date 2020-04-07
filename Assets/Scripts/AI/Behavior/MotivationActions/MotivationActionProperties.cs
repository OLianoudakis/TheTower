using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI.KnowledgeBase;
using AI.KnowledgeBase.Parser;
using System.Linq;
using Utils.EditorProperties;

namespace AI.Behavior.MotivationActions
{
    public class MotivationActionProperties : MonoBehaviour
    {
        [ReadOnly]
        private string m_priorityDescription = "Priority for action used when personality model disabled.";

        [SerializeField]
        private int m_priority = 0;

        [SerializeField]
        private MotivationGain m_motivationGain;

        [SerializeField]
        private KnowledgeBaseRuleset m_actionTriggerRules = KnowledgeBaseRulesEngine.GetKnowledgebaseRuleSet();

        private KnowledgeBase.KnowledgeBase m_knowledgeBase;
        private Exp m_abstractSyntaxTree;
        private LogicalOperationsValidator m_logicalOperationsValidator;

        public int priority
        {
            get { return m_priority; }
        }

        public MotivationGain motivationGain
        {
            get { return m_motivationGain; }
            set { m_motivationGain = value; }
        }

        public bool CanBeTriggered()
        {
            // in case of one rule we can just check the rule directly
            if (m_actionTriggerRules.knowledgeBaseRules.Length == 1)
            {
                if (!KnowledgeBaseRulesEngine.IsRuleTrue(
                    m_knowledgeBase, m_actionTriggerRules.knowledgeBaseRules[0].m_name, m_actionTriggerRules.knowledgeBaseRules[0].m_value))
                {
                    return false;
                }
            }
            // in case of more rules validate the abstract syntax tree
            else if (m_abstractSyntaxTree != null)
            {
                return m_logicalOperationsValidator.ValidateRules(m_abstractSyntaxTree);
            }
            // for no rules defined its always true
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
            m_logicalOperationsValidator = new LogicalOperationsValidator(m_knowledgeBase);
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }
    }
}
