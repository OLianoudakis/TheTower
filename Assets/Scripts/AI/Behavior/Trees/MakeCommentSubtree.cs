using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
using AI.Behavior.MotivationActions;

namespace AI.Behavior.Trees
{
    public struct MakeCommentSubtree
    {
        public Node m_root;

        private Root m_behaviorTreeRoot;
        private TextMesh m_textMesh;
        private MotivationActionsCommentsCatalogue m_catalogue;
        private PersonalityType m_personalityType;
        private float m_chanceToComment;
        private float m_currentChanceToComment;
        private double m_lastElapsedTime;

        public void Create(
            Root behaviorTreeRoot, 
            MotivationActionsCommentsCatalogue catalogue,
            TextMesh textMesh,
            PersonalityType personalityType,
            float chanceToComment = 85.0f)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_textMesh = textMesh;
            m_catalogue = catalogue;
            m_personalityType = personalityType;
            m_chanceToComment = chanceToComment;
            m_currentChanceToComment = 0.0f;
            m_lastElapsedTime = 0.0;

            m_root =
                new Action(PickComment);
        }

        private void PickComment()
        {
            m_textMesh.text = "";
            float rollCommentChance = UnityEngine.Random.Range(0.0f, m_chanceToComment);
            if (rollCommentChance > (100.0f - m_currentChanceToComment))
            {
                m_textMesh.text = m_catalogue.GetComment(m_personalityType);
                m_currentChanceToComment = 0.0f;
            }
            else
            {
                m_currentChanceToComment += 10.0f;
            }
            //m_behaviorTreeRoot.Blackboard.Set("commentAvailable", false);
        }
    }
}
