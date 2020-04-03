using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior.MotivationActions
{
    public class MotivationActionsCommentsCatalogue : MonoBehaviour
    {
        [SerializeField]
        private MotivationActionComments m_motivationActionComments;

        public string GetComment(PersonalityType personalityType)
        {
            MotivationActionCommentsEntry entry = m_motivationActionComments.m_motivationActionCommentsEntries[(int)personalityType];
            int chosenResponse = Random.Range(1, entry.m_possibleComments.Length) - 1;
            return entry.m_possibleComments[chosenResponse];
        }
    }
    public enum PersonalityType
    {
        Fearful = 0,
        Brave = 1,
        Pedantic = 2,
        Lazy = 3
    }
}
