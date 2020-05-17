using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.KnowledgeBase.Parser
{
    public class LogicalOperationsValidator : Visitor
    {
        private KnowledgeBase m_knowledgeBase;

        public LogicalOperationsValidator(KnowledgeBase knowledgeBase)
        {
            m_knowledgeBase = knowledgeBase;
        }

        public bool ValidateRules(Exp exp)
        {
            if (exp.GetType() != ExpType.Root)
            {
                throw new System.Exception("Root node must be of type Root!");
            }
            return visitRoot((Root)exp);
        }

        public bool visitRoot(Root exp)
        {
            return exp.m_exp.accept(this);
        }

        public bool visitBracket(Bracket exp)
        {
            return exp.m_exp.accept(this);
        }

        public bool visitRule(Rule exp)
        {
            return KnowledgeBaseRulesEngine.IsRuleTrue(m_knowledgeBase, exp.m_rule.m_name, exp.m_rule.m_value);
        }

        public bool visitNot(Not exp)
        {
            return !(exp.m_exp.accept(this));
        }

        public bool visitOr(Or exp)
        {
            bool first = exp.m_exp1.accept(this);
            bool second = exp.m_exp2.accept(this);
            if (first || second)
            {
                return true;
            }
            return false;
        }

        public bool visitAnd(And exp)
        {
            bool first = exp.m_exp1.accept(this);
            bool second = exp.m_exp2.accept(this);
            if (first && second)
            {
                return true;
            }
            return false;
        }
    }
}
