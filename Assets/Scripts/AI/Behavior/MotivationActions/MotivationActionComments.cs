using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utils.EditorProperties;

namespace AI.Behavior.MotivationActions
{
    [Serializable]
    public struct MotivationActionCommentsEntry
    {
        //[ReadOnly]
        public PersonalityType m_personalityType;
        public string[] m_possibleComments;

        public MotivationActionCommentsEntry(PersonalityType personalityType)
        {
            m_personalityType = personalityType;
            m_possibleComments = null;
        }
    }

    [CreateAssetMenu(fileName = "New Motivation Action Comments", menuName = "Motivation Action Comments")]
    public class MotivationActionComments : ScriptableObject
    {
        [SerializeField]
        public MotivationActionCommentsEntry[] m_motivationActionCommentsEntries
            = new MotivationActionCommentsEntry[4]
            {
                new MotivationActionCommentsEntry(PersonalityType.Fearful),
                new MotivationActionCommentsEntry(PersonalityType.Brave),
                new MotivationActionCommentsEntry(PersonalityType.Pedantic),
                new MotivationActionCommentsEntry(PersonalityType.Lazy)
            };
    }
}
