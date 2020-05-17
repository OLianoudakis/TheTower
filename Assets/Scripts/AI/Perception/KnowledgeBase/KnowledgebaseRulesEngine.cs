using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Utils.EditorProperties;
using UnityEditor;

namespace AI.KnowledgeBase
{
    public interface CustomType
    {
        // NOTE: it doesnt work like classic style operations, where the callee is the lhs,
        // here, for ease of use with unity editor, callee is actually rhs
        bool Compare(object other);
        Type GetCustomType();
    }

    [Serializable]
    public class IntType : CustomType
    {
        public enum OperatorType
        {
            Equals,
            NotEquals,
            LowerThan,
            HigherThan
        }

        public int m_value;
        public OperatorType m_operator;

        public bool Compare(object other)
        {
            if (m_operator == OperatorType.Equals)
            {
                if ((int)other == m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.NotEquals)
            {
                if ((int)other != m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.LowerThan)
            {
                if ((int)other < m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.HigherThan)
            {
                if ((int)other > m_value)
                {
                    return true;
                }
                return false;
            }
            // undefined operator
            return false;
        }

        public Type GetCustomType()
        {
            return typeof(int);
        }
    }

    [Serializable]
    public class FloatType : CustomType
    {
        public enum OperatorType
        {
            Equals,
            NotEquals,
            LowerThan,
            HigherThan
        }

        public float m_value;
        public OperatorType m_operator;

        public bool Compare(object other)
        {
            if (m_operator == OperatorType.Equals)
            {
                if ((float)other == m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.NotEquals)
            {
                if ((float)other != m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.LowerThan)
            {
                if ((float)other < m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.HigherThan)
            {
                if ((float)other > m_value)
                {
                    return true;
                }
                return false;
            }
            // undefined operator
            return false;
        }

        public Type GetCustomType()
        {
            return typeof(float);
        }
    }

    [Serializable]
    public class BoolType : CustomType
    {
        public enum OperatorType
        {
            Equals,
            NotEquals
        }

        public bool m_value;
        public OperatorType m_operator;

        public bool Compare(object other)
        {
            if (m_operator == OperatorType.Equals)
            {
                if ((bool)other == m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.NotEquals)
            {
                if ((bool)other != m_value)
                {
                    return true;
                }
                return false;
            }
            // undefined operator
            return false;
        }

        public Type GetCustomType()
        {
            return typeof(bool);
        }
    }

    [Serializable]
    public class StringType : CustomType
    {
        public enum OperatorType
        {
            Equals,
            NotEquals,
            IsSubstring
        }

        public string m_value;
        public OperatorType m_operator;

        public bool Compare(object other)
        {
            if (m_operator == OperatorType.Equals)
            {
                if (((string)other).Equals(m_value))
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.NotEquals)
            {
                if (!((string)other).Equals(m_value))
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.IsSubstring)
            {
                if (((string)other).Contains(m_value))
                {
                    return true;
                }
                return false;
            }
            // undefined operator
            return false;
        }

        public Type GetCustomType()
        {
            return typeof(string);
        }
    }

    [Serializable]
    public class GameObjectType : CustomType
    {
        public enum OperatorType
        {
            Equals,
            NotEquals,
            IsNull,
            IsNotNull
        }

        public GameObject m_value;
        public OperatorType m_operator;

        public bool Compare(object other)
        {
            if (m_operator == OperatorType.Equals)
            {
                if ((GameObject)other == m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.NotEquals)
            {
                if ((GameObject)other != m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.IsNull)
            {
                if (!(GameObject)other)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.IsNotNull)
            {
                if ((GameObject)other)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public Type GetCustomType()
        {
            return typeof(GameObject);
        }
    }

    [Serializable]
    public class TransformType : CustomType
    {
        public enum OperatorType
        {
            Equals,
            NotEquals,
            IsNull,
            IsNotNull
        }

        public Transform m_value;
        public OperatorType m_operator;

        public bool Compare(object other)
        {
            if (m_operator == OperatorType.Equals)
            {
                if ((Transform)other == m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.NotEquals)
            {
                if ((Transform)other != m_value)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.IsNull)
            {
                if (!(Transform)other)
                {
                    return true;
                }
                return false;
            }
            else if (m_operator == OperatorType.IsNotNull)
            {
                if ((Transform)other)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public Type GetCustomType()
        {
            return typeof(Transform);
        }
    }

    [Serializable]
    public struct KnowledgeBaseRule
    {
        //[ReadOnly]
        public string m_name;

        //[ReadOnly]
        public int m_index;

        public bool m_useRule;

        [SerializeReference]
        public CustomType m_value;

        public KnowledgeBaseRule(string name, int index, Type type)
        {
            m_name = name;
            m_index = index;
            m_value = null;
            m_useRule = false;
            if (type == typeof(int))
            {
                m_value = new IntType();
            }
            else if (type == typeof(float))
            {
                m_value = new FloatType();
            }
            else if (type == typeof(bool))
            {
                m_value = new BoolType();
            }
            else if (type == typeof(string))
            {
                m_value = new StringType();
            }
            else if (type == typeof(GameObject))
            {
                m_value = new GameObjectType();
            }
            else if (type == typeof(Transform))
            {
                m_value = new TransformType();
            }
        }
    }

    [Serializable]
    public struct LogicalOperationPrimitive
    {
        public enum LogicalOperatorType
        {
            LeftBracket,
            RightBracket,
            AND,
            OR,
            NOT
        }

        public bool m_isOperator;
        public LogicalOperatorType m_operator;
        public bool m_isOperand;
        public int m_indexOfOperand;
    }

    [Serializable]
    public struct LogicalOperationSet
    {
        //[ReadOnly]
        public int m_maxIndexValue;

        [SerializeField]
        public LogicalOperationPrimitive[] m_logicalOperationPrimitives;

        public LogicalOperationSet(int numberOfRules)
        {
            m_maxIndexValue = numberOfRules;
            m_logicalOperationPrimitives = null;
        }   
    }

    [Serializable]
    public struct KnowledgeBaseRuleset
    {
        [SerializeField]
        private LogicalOperationSet m_logicalOperationSet;

        [SerializeField]
        private KnowledgeBaseRule[] m_knowledgeBaseRules;

        public LogicalOperationSet logicalOperationSet
        {
            get { return m_logicalOperationSet; }
            set { m_logicalOperationSet = value; }
        }

        public KnowledgeBaseRule[] knowledgeBaseRules
        {
            get { return m_knowledgeBaseRules; }
            set { m_knowledgeBaseRules = value; }
        }
    }

    public class KnowledgeBaseRulesEngine
    {
        public static KnowledgeBaseRuleset GetKnowledgebaseRuleSet()
        {
            Type kbType = typeof(KnowledgeBase);
            Type mbType = typeof(MonoBehaviour);
            PropertyInfo[] kbProperties = kbType.GetProperties();
            PropertyInfo[] mbProperties = mbType.GetProperties();
            List<KnowledgeBaseRule> propertiesList = new List<KnowledgeBaseRule>();
            for (int i = 0; i < kbProperties.Length; i++)
            {
                bool skip = false;
                foreach (PropertyInfo mbProperty in mbProperties)
                {
                    if (kbProperties[i].Name.Equals(mbProperty.Name))
                    {
                        skip = true;
                        continue;
                    }
                }
                if (!skip)
                {
                    KnowledgeBaseRule kbRule = new KnowledgeBaseRule(kbProperties[i].Name, i, kbProperties[i].PropertyType);
                    propertiesList.Add(kbRule);
                }
            }
            KnowledgeBaseRuleset ruleset = new KnowledgeBaseRuleset();
            ruleset.knowledgeBaseRules = propertiesList.ToArray();
            ruleset.logicalOperationSet = new LogicalOperationSet(ruleset.knowledgeBaseRules.Length);
            return ruleset;
        }

        public static bool IsRuleTrue(KnowledgeBase knowledgeBase, string kbPropertyName, CustomType value)
        {
            Type kbType = knowledgeBase.GetType();
            PropertyInfo pInfo = kbType.GetProperty(kbPropertyName);
            Type propertyType = pInfo.PropertyType;
            if (propertyType == value.GetCustomType())
            {
                object kbValue = pInfo.GetValue(knowledgeBase);
                return value.Compare(kbValue);
            }
            return false;
        }
    }
}
