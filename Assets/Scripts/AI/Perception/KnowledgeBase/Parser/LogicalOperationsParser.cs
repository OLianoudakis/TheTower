using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AI.KnowledgeBase.Parser
{
    public class LogicalOperationsParser : Visitor
    {
        List<Environment> m_rulesetStack = new List<Environment>();
        KnowledgeBaseRule[] m_knowledgeBaseRules;

        public LogicalOperationsParser(LogicalOperationPrimitive[] ruleset, KnowledgeBaseRule[] knowledgeBaseRules)
        {
            Environment env = new Environment();
            env.logicalOperationPrimitives = ruleset;
            m_rulesetStack.Add(env);
            m_knowledgeBaseRules = knowledgeBaseRules;
        }

        public Exp ParseRuleset()
        {
            if (m_rulesetStack[0].logicalOperationPrimitives.Length != 0)
            {
                Root root = new Root();
                visitRoot(root);
                return root;
            }
            return null;
        }

        public bool visitRoot(Root exp)
        {
            int index = m_rulesetStack[0].m_currentIndex;
            if (m_rulesetStack[0].logicalOperationPrimitives[0].m_isOperator)
            {
                if (m_rulesetStack[0].logicalOperationPrimitives[0].m_isOperand)
                {
                    throw new System.Exception("Cannot use operand and operator at the same time!");
                }
                if (m_rulesetStack[0].logicalOperationPrimitives[0].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
                {
                    throw new System.Exception("Cannot start logical operation set with RightBracket operator!");
                }
                if (m_rulesetStack[0].logicalOperationPrimitives[0].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
                {
                    Bracket bracket = new Bracket();
                    bracket.m_parent = exp;
                    visitBracket(bracket);
                }
                else if (m_rulesetStack[0].logicalOperationPrimitives[0].m_operator == LogicalOperationPrimitive.LogicalOperatorType.NOT)
                {
                    Not not = new Not();
                    not.m_parent = exp;
                    visitNot(not);
                }
                else if (m_rulesetStack[0].logicalOperationPrimitives[0].m_operator == LogicalOperationPrimitive.LogicalOperatorType.AND)
                {
                    throw new System.Exception("Cannot start logical operation set with AND operator!");
                }
                else if (m_rulesetStack[0].logicalOperationPrimitives[0].m_operator == LogicalOperationPrimitive.LogicalOperatorType.OR)
                {
                    throw new System.Exception("Cannot start logical operation set with OR operator!");
                }
            }
            else if (m_rulesetStack[0].logicalOperationPrimitives[0].m_isOperand)
            {
                Rule rule = new Rule();
                rule.m_parent = exp;
                visitRule(rule);
            }
            else
            {
                throw new System.Exception("Logical Operation Primitive not chosen!");
            }

            return true;
        }

        public bool visitBracket(Bracket exp)
        {
            Environment env = m_rulesetStack[m_rulesetStack.Count - 1];
            int currentIndex = ++env.m_currentIndex;
            bool isLeftBracket = false;
            if (env.logicalOperationPrimitives[currentIndex - 1].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
            {
                if (currentIndex == env.logicalOperationPrimitives.Length)
                {
                    throw new System.Exception("Incomplete Logical Operation Set provided!");
                }
                isLeftBracket = true;
                int rightBracketIndex = SearchForRightBracket();
                env.m_currentIndex = rightBracketIndex + 1; // move the index of this environment after the brackets
                env.m_previousExpression = exp; // save this bracket in bottom environment

                Environment newEnv = new Environment();
                int takeLength = rightBracketIndex - currentIndex + 1; // +1 to take the RightBracket too
                newEnv.logicalOperationPrimitives = m_rulesetStack[m_rulesetStack.Count - 1]
                    .logicalOperationPrimitives
                    .Skip(currentIndex)
                    .Take(takeLength)
                    .ToArray();
                m_rulesetStack.Add(newEnv);
                env = newEnv;
            }
            else if (env.logicalOperationPrimitives[currentIndex - 1].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
            {
                m_rulesetStack.RemoveAt(m_rulesetStack.Count - 1);
                env = m_rulesetStack[m_rulesetStack.Count - 1];

            }
            currentIndex = env.m_currentIndex;
            if ((currentIndex >= env.logicalOperationPrimitives.Length)
                || (env.logicalOperationPrimitives[currentIndex].m_isOperator
                && (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)))
            {
                if (((Bracket)env.m_previousExpression).m_parent != null)
                {
                    Exp parentExpression = ((Bracket)env.m_previousExpression).m_parent;
                    Exp childExpression = env.m_previousExpression;
                    if (parentExpression.GetType() == ExpType.Not)
                    {
                        parentExpression = ((Not)parentExpression).m_parent;
                        childExpression = (Not)parentExpression;
                        ((Not)exp.m_parent).m_exp = env.m_previousExpression;
                    }

                    if (parentExpression.GetType() == ExpType.Bracket)
                    {
                        ((Bracket)parentExpression).m_exp = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.Root)
                    {
                        ((Root)parentExpression).m_exp = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.Not)
                    {
                        ((Not)parentExpression).m_exp = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.Or)
                    {
                        ((Or)parentExpression).m_exp2 = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.And)
                    {
                        ((And)parentExpression).m_exp2 = childExpression;
                    }
                }
                if (currentIndex >= env.logicalOperationPrimitives.Length)
                {
                    return true;
                }
            }
            if (env.logicalOperationPrimitives[currentIndex].m_isOperator)
            {
                if (env.logicalOperationPrimitives[currentIndex].m_isOperand)
                {
                    throw new System.Exception("Cannot use operand and operator at the same time!");
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
                {
                    if (isLeftBracket)
                    {
                        throw new System.Exception("Cannot use empty brackets!");
                    }
                    Bracket bracket = new Bracket();
                    visitBracket(bracket);
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
                {
                    if (!isLeftBracket)
                    {
                        throw new System.Exception("Cannot use consecutive brackets without operator!");
                    }
                    Bracket bracket = new Bracket();
                    bracket.m_parent = exp;
                    visitBracket(bracket);
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.NOT)
                {
                    Not not = new Not();
                    not.m_parent = exp;
                    visitNot(not);
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.AND)
                {
                    if (isLeftBracket)
                    {
                        throw new System.Exception("Cannot use AND operation after LeftBracket operand!");
                    }
                    And and = new And();

                    Exp parentExpression = ((Bracket)env.m_previousExpression).m_parent;
                    if (parentExpression.GetType() == ExpType.Not)
                    {
                        parentExpression = ((Not)parentExpression).m_parent;
                        ((Not)parentExpression).m_exp = env.m_previousExpression;
                        env.m_previousExpression = parentExpression;
                    }

                    if (parentExpression.GetType() == ExpType.Bracket)
                    {
                        ((Bracket)parentExpression).m_exp = and;
                    }
                    else if (parentExpression.GetType() == ExpType.Root)
                    {
                        ((Root)parentExpression).m_exp = and;
                    }
                    else if (parentExpression.GetType() == ExpType.Not)
                    {
                        ((Not)parentExpression).m_exp = and;
                    }
                    else if (parentExpression.GetType() == ExpType.Or)
                    {
                        ((Or)parentExpression).m_exp2 = and;
                    }
                    else if (parentExpression.GetType() == ExpType.And)
                    {
                        ((And)parentExpression).m_exp2 = and;
                    }
                    visitAnd(and);
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.OR)
                {
                    if (isLeftBracket)
                    {
                        throw new System.Exception("Cannot use OR operation after LeftBracket operand!");
                    }
                    Or or = new Or();

                    Exp parentExpression = ((Bracket)env.m_previousExpression).m_parent;
                    if (parentExpression.GetType() == ExpType.Not)
                    {
                        parentExpression = ((Not)parentExpression).m_parent;
                        ((Not)parentExpression).m_exp = env.m_previousExpression;
                        env.m_previousExpression = parentExpression;
                    }

                    if (parentExpression.GetType() == ExpType.Bracket)
                    {
                        ((Bracket)parentExpression).m_exp = or;
                    }
                    else if (parentExpression.GetType() == ExpType.Root)
                    {
                        ((Root)parentExpression).m_exp = or;
                    }
                    else if (parentExpression.GetType() == ExpType.Not)
                    {
                        ((Not)parentExpression).m_exp = or;
                    }
                    else if (parentExpression.GetType() == ExpType.Or)
                    {
                        ((Or)parentExpression).m_exp2 = or;
                    }
                    else if (parentExpression.GetType() == ExpType.And)
                    {
                        ((And)parentExpression).m_exp2 = or;
                    }
                    visitOr(or);
                }
            }
            else if (env.logicalOperationPrimitives[currentIndex].m_isOperand)
            {
                if (!isLeftBracket)
                {
                    throw new System.Exception("Cannot use operand after RightBracket operator!");
                }
                Rule rule = new Rule();
                rule.m_parent = exp;
                visitRule(rule);
            }
            else
            {
                throw new System.Exception("Logical Operation Primitive not chosen!");
            }

            return true;
        }

        public bool visitRule(Rule exp)
        {
            Environment env = m_rulesetStack[m_rulesetStack.Count - 1];
            int ruleIndex = env.logicalOperationPrimitives[env.m_currentIndex++].m_indexOfOperand;
            exp.m_rule = m_knowledgeBaseRules[ruleIndex];
            if (!exp.m_rule.m_useRule)
            {
                throw new System.Exception("Selected operand not set to be used!");
            }
            env.m_previousExpression = exp;
            int currentIndex = env.m_currentIndex;
            if ((currentIndex >= env.logicalOperationPrimitives.Length)
                || (env.logicalOperationPrimitives[currentIndex].m_isOperator
                && (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)))
            {
                if (exp.m_parent != null)
                {
                    Exp parentExpression = exp.m_parent;
                    Exp childExpression = exp;
                    if (parentExpression.GetType() == ExpType.Not)
                    {
                        parentExpression = ((Not)exp.m_parent).m_parent;
                        childExpression = (Not)exp.m_parent;
                        ((Not)exp.m_parent).m_exp = exp;
                    }

                    if (parentExpression.GetType() == ExpType.Bracket)
                    {
                        ((Bracket)parentExpression).m_exp = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.Root)
                    {
                        ((Root)parentExpression).m_exp = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.Not)
                    {
                        ((Not)parentExpression).m_exp = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.Or)
                    {
                        ((Or)parentExpression).m_exp2 = childExpression;
                    }
                    else if (parentExpression.GetType() == ExpType.And)
                    {
                        ((And)parentExpression).m_exp2 = childExpression;
                    }
                }
                if (currentIndex >= env.logicalOperationPrimitives.Length)
                {
                    return true;
                }
            }
            if (env.logicalOperationPrimitives[currentIndex].m_isOperator)
            {
                if (env.logicalOperationPrimitives[currentIndex].m_isOperand)
                {
                    throw new System.Exception("Cannot use operand and operator at the same time!");
                }
                if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
                {
                    Bracket bracket = new Bracket();
                    visitBracket(bracket);
                }
                if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
                {
                    throw new System.Exception("Cannot use LeftBracket operator after operand!");
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.NOT)
                {
                    throw new System.Exception("Cannot use Not operator after operand!");
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.AND)
                {
                    And and = new And();
                    Exp parentExpression = exp.m_parent;
                    if (parentExpression.GetType() == ExpType.Not)
                    {
                        parentExpression = ((Not)exp.m_parent).m_parent;
                        ((Not)exp.m_parent).m_exp = exp;
                        env.m_previousExpression = exp.m_parent;
                    }

                    if (parentExpression.GetType() == ExpType.Bracket)
                    {
                        ((Bracket)parentExpression).m_exp = and;
                    }
                    else if (parentExpression.GetType() == ExpType.Root)
                    {
                        ((Root)parentExpression).m_exp = and;
                    }
                    else if (parentExpression.GetType() == ExpType.Not)
                    {
                        ((Not)parentExpression).m_exp = and;
                    }
                    else if (parentExpression.GetType() == ExpType.Or)
                    {
                        ((Or)parentExpression).m_exp2 = and;
                    }
                    else if (parentExpression.GetType() == ExpType.And)
                    {
                        ((And)parentExpression).m_exp2 = and;
                    }
                    visitAnd(and);
                }
                else if (env.logicalOperationPrimitives[currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.OR)
                {
                    Or or = new Or();
                    Exp parentExpression = exp.m_parent;
                    if (exp.m_parent.GetType() == ExpType.Not)
                    {
                        parentExpression = ((Not)exp.m_parent).m_parent;
                        ((Not)exp.m_parent).m_exp = exp;
                        env.m_previousExpression = exp.m_parent;
                    }

                    if (parentExpression.GetType() == ExpType.Bracket)
                    {
                        ((Bracket)parentExpression).m_exp = or;
                    }
                    else if (parentExpression.GetType() == ExpType.Root)
                    {
                        ((Root)parentExpression).m_exp = or;
                    }
                    else if (parentExpression.GetType() == ExpType.Not)
                    {
                        ((Not)parentExpression).m_exp = or;
                    }
                    else if (parentExpression.GetType() == ExpType.Or)
                    {
                        ((Or)parentExpression).m_exp2 = or;
                    }
                    else if (parentExpression.GetType() == ExpType.And)
                    {
                        ((And)parentExpression).m_exp2 = or;
                    }
                    visitOr(or);
                }
            }
            else if (env.logicalOperationPrimitives[currentIndex].m_isOperand)
            {
                throw new System.Exception("Two consecutive operands not allowed!");
            }
            else
            {
                throw new System.Exception("Logical Operation Primitive not chosen!");
            }

            return true;
        }


        public bool visitNot(Not exp)
        {
            Environment env = m_rulesetStack[m_rulesetStack.Count - 1];
            int currentIndex = ++env.m_currentIndex;
            if (env.m_currentIndex >= env.logicalOperationPrimitives.Length)
            {
                throw new System.Exception("Cannot finish logical operation set with NOT operator!");
            }
            if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperator)
            {
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperand)
                {
                    throw new System.Exception("Cannot use operand and operator at the same time!");
                }
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
                {
                    throw new System.Exception("Cannot use RightBracket operator after NOT operator!");
                }
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
                {
                    Bracket bracket = new Bracket();
                    exp.m_exp = bracket;
                    visitBracket(bracket);
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.NOT)
                {
                    throw new System.Exception("Cannot use double negation operation!");
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.AND)
                {
                    throw new System.Exception("Cannot use AND operator after NOT operator!");
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.OR)
                {
                    throw new System.Exception("Cannot use OR operator after NOT operator!");
                }
            }
            else if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperand)
            {
                Rule rule = new Rule();
                rule.m_parent = exp;
                visitRule(rule);
            }
            else
            {
                throw new System.Exception("Logical Operation Primitive not chosen!");
            }

            return true;
        }

        public bool visitOr(Or exp)
        {
            Environment env = m_rulesetStack[m_rulesetStack.Count - 1];
            if (env.m_previousExpression == null)
            {
                throw new System.Exception("Cannot use OR operator without left hand side operand!");
            }
            exp.m_exp1 = env.m_previousExpression;
            env.m_previousExpression = null;
            int currentIndex = ++env.m_currentIndex;
            if (env.m_currentIndex >= env.logicalOperationPrimitives.Length)
            {
                throw new System.Exception("Cannot finish logical operation set with OR operator!");
            }
            if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperator)
            {
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperand)
                {
                    throw new System.Exception("Cannot use operand and operator at the same time!");
                }
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
                {
                    throw new System.Exception("Cannot use RightBracket operator after OR operator!");
                }
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
                {
                    Bracket bracket = new Bracket();
                    bracket.m_parent = exp;
                    visitBracket(bracket);
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.NOT)
                {
                    Not not = new Not();
                    not.m_parent = exp;
                    visitNot(not);
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.AND)
                {
                    throw new System.Exception("Cannot use AND operator after OR operator!");
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.OR)
                {
                    throw new System.Exception("Cannot use OR operator after OR operator!");
                }
            }
            else if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperand)
            {
                Rule rule = new Rule();
                rule.m_parent = exp;
                visitRule(rule);
            }
            else
            {
                throw new System.Exception("Logical Operation Primitive not chosen!");
            }

            return true;
        }

        public bool visitAnd(And exp)
        {
            Environment env = m_rulesetStack[m_rulesetStack.Count - 1];
            if (env.m_previousExpression == null)
            {
                throw new System.Exception("Cannot use AND operator without left hand side operand!");
            }
            exp.m_exp1 = env.m_previousExpression;
            env.m_previousExpression = null;
            int currentIndex = ++env.m_currentIndex;
            if (env.m_currentIndex >= env.logicalOperationPrimitives.Length)
            {
                throw new System.Exception("Cannot finish logical operation set with AND operator!");
            }
            if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperator)
            {
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperand)
                {
                    throw new System.Exception("Cannot use operand and operator at the same time!");
                }
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
                {
                    throw new System.Exception("Cannot use RightBracket operator after AND operator!");
                }
                if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
                {
                    Bracket bracket = new Bracket();
                    bracket.m_parent = exp;
                    visitBracket(bracket);
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.NOT)
                {
                    Not not = new Not();
                    not.m_parent = exp;
                    visitNot(not);
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.AND)
                {
                    throw new System.Exception("Cannot use AND operator after AND operator!");
                }
                else if (env.logicalOperationPrimitives[env.m_currentIndex].m_operator == LogicalOperationPrimitive.LogicalOperatorType.OR)
                {
                    throw new System.Exception("Cannot use OR operator after AND operator!");
                }
            }
            else if (env.logicalOperationPrimitives[env.m_currentIndex].m_isOperand)
            {
                Rule rule = new Rule();
                rule.m_parent = exp;
                visitRule(rule);
            }
            else
            {
                throw new System.Exception("Logical Operation Primitive not chosen!");
            }

            return true;
        }

        private int SearchForRightBracket()
        {
            int leftBracketCount = 0;
            LogicalOperationPrimitive[] ruleset = m_rulesetStack[m_rulesetStack.Count - 1].logicalOperationPrimitives;
            for (int i = m_rulesetStack[m_rulesetStack.Count - 1].m_currentIndex; i < ruleset.Length; i++)
            {
                if (ruleset[i].m_isOperator)
                {
                    if (ruleset[i].m_operator == LogicalOperationPrimitive.LogicalOperatorType.LeftBracket)
                    {
                        ++leftBracketCount;
                    }
                    else if (ruleset[i].m_operator == LogicalOperationPrimitive.LogicalOperatorType.RightBracket)
                    {
                        if (leftBracketCount == 0)
                        {
                            return i;
                        }
                        --leftBracketCount;
                    }
                }
            }
            return 0;
        }
    }
}
