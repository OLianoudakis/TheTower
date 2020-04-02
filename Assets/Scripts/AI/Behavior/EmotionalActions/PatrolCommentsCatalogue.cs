using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI.Behavior.EmotionalActions
{
    public class PatrolCommentsCatalogue : MonoBehaviour
    {
        public string GetPatrolComment(PatrolCommentType patrolCommentType)
        {
            switch(patrolCommentType)
            {
                case PatrolCommentType.Fearful:
                    return FearfulComment();
                default:
                    return "hmm...";
            }
        }

        private string FearfulComment()
        {
            string[] responses = new string[] { "Oh... why me?",
                                                "I hope Master let's " +
                                                "me back in the quarters",
                                                "Oh man...",
                                                "Was it always " +
                                                "this dark here?"};
            int chosenResponse = Random.Range(1, responses.Length) - 1;
            return responses[chosenResponse];
        }
    }
    public enum PatrolCommentType
    {
        Fearful,
        Brave,
        Pedantic,
        Lazy
    }
}
