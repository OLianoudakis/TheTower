using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.KnowledgeBase.Parser
{
    public interface Visitable
    {
        bool accept(Visitor v);
    };

    public enum ExpType
    {
        Root,
        Bracket,
        And,
        Or,
        Not,
        Rule
    }

    public interface Exp : Visitable
    {
        ExpType GetType();
    };

    public class Root : Exp
    {
        public Exp m_exp;
        public bool accept(Visitor v)
        {
            return v.visitRoot(this);
        }
        public ExpType GetType()
        {
            return ExpType.Root;
        }
    };

    public class Bracket : Exp
    {
        public Exp m_exp;
        public Exp m_parent = null;
        public bool accept(Visitor v)
        {
            return v.visitBracket(this);
        }
        public ExpType GetType()
        {
            return ExpType.Bracket;
        }
    };

    public class Not : Exp
    {
        public Exp m_exp;
        public Exp m_parent = null;
        public bool accept(Visitor v)
        {
            return v.visitNot(this);
        }
        public ExpType GetType()
        {
            return ExpType.Not;
        }
    };

    public class And : Exp
    {
	    public Exp m_exp1;
        public Exp m_exp2;
        public bool accept(Visitor v)
        {
            return v.visitAnd(this);
        }
        public ExpType GetType()
        {
            return ExpType.And;
        }
    };

    public class Or : Exp
    {
        public Exp m_exp1;
        public Exp m_exp2;
        public bool accept(Visitor v)
        {
            return v.visitOr(this);
        }
        public ExpType GetType()
        {
            return ExpType.Or;
        }
    };

    public class Rule : Exp
    {
        public Exp m_parent = null;
        public KnowledgeBaseRule m_rule;
        public bool accept(Visitor v)
        {
            return v.visitRule(this);
        }
        public ExpType GetType()
        {
            return ExpType.Rule;
        }
    };

    public interface Visitor
    {
        bool visitRule(Rule exp);
        bool visitRoot(Root exp);
        bool visitBracket(Bracket exp);
        bool visitNot(Not exp);
        bool visitOr(Or exp);
        bool visitAnd(And exp);
    };

    public class Environment
    {
        public LogicalOperationPrimitive[] logicalOperationPrimitives;
        public int m_currentIndex = 0;
        public Exp m_previousExpression = null;
    }
}
