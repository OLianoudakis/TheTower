using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Utils.EditorProperties
{
    //public class ReadOnlyAttribute : PropertyAttribute
    //{

    //}

    //[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    //public class ReadOnlyDrawer : PropertyDrawer
    //{
    //    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    //    {
    //        return EditorGUI.GetPropertyHeight(property, label, true);
    //    }

    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        GUI.enabled = false;
    //        EditorGUI.PropertyField(position, property, label, true);
    //        GUI.enabled = true;
    //    }
    //}

    //public class EnabledIfAttribute : PropertyAttribute
    //{
    //    public string m_fieldName;
    //    public string m_otherFieldName;

    //    public EnabledIfAttribute(string fieldName, string otherFieldName)
    //    {
    //        m_fieldName = fieldName;
    //        m_otherFieldName = otherFieldName;
    //    }
    //}

    //[CustomPropertyDrawer(typeof(EnabledIfAttribute))]
    //public class EnabledIfDrawer : PropertyDrawer
    //{
    //    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    //    {
    //        EnabledIfAttribute enabledIf = attribute as EnabledIfAttribute;
    //        bool isOtherEnabled = false;
    //        isOtherEnabled = EditorGUILayout.Toggle(enabledIf.m_otherFieldName, isOtherEnabled);
            
    //        using (new EditorGUI.DisabledScope(isOtherEnabled == true))
    //        {
    //            EditorGUILayout.Toggle(enabledIf.m_fieldName, false);
    //        }
    //    }
    //}
}
