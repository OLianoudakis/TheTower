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
        private FloatingTextBehavior m_textMesh;
        private MotivationActionsCommentsCatalogue m_catalogue;
        private PersonalityType m_personalityType;

        public void Create(
            Root behaviorTreeRoot, 
            MotivationActionsCommentsCatalogue catalogue,
            FloatingTextBehavior textMesh,
            PersonalityType personalityType)
        {
            m_behaviorTreeRoot = behaviorTreeRoot;
            m_textMesh = textMesh;
            m_catalogue = catalogue;
            m_personalityType = personalityType;

            m_root =
                new Action(PickComment);
        }

        private void PickComment()
        {
            int rollCommentChance = UnityEngine.Random.Range(0, 2);
            if (rollCommentChance == 1)
            {
                m_textMesh.ChangeText(m_catalogue.GetComment(m_personalityType));
            }
        }
    }
}
