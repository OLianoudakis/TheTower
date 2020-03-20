using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using Utils.EditorProperties;

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
        [ReadOnly]
        public string m_name;

        public bool m_useRule;

        [SerializeReference]
        public CustomType m_value;

        public KnowledgeBaseRule(string name, Type type)
        {
            m_name = name;
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

    public class KnowledgeBaseRulesEngine
    {
        public static KnowledgeBaseRule[] GetKnowledgebaseRuleSet()
        {
            Type kbType = typeof(KnowledgeBase);
            Type mbType = typeof(MonoBehaviour);
            PropertyInfo[] kbProperties = kbType.GetProperties();
            PropertyInfo[] mbProperties = mbType.GetProperties();
            List<KnowledgeBaseRule> propertiesList = new List<KnowledgeBaseRule>();
            foreach (PropertyInfo kbProperty in kbProperties)
            {
                bool skip = false;
                foreach (PropertyInfo mbProperty in mbProperties)
                {
                    if (kbProperty.Name.Equals(mbProperty.Name))
                    {
                        skip = true;
                        continue;
                    }
                }
                if (!skip)
                {
                    KnowledgeBaseRule kbRule = new KnowledgeBaseRule(kbProperty.Name, kbProperty.PropertyType);
                    propertiesList.Add(kbRule);
                }
            }
            return propertiesList.ToArray();
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
