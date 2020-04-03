using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

namespace AI.Behavior.Trees.Tasks
{
    public class MakeComment : Task
    {
        private TextMesh m_textMesh;
        private string m_blackBoardKeyText;

        public MakeComment(TextMesh textMesh, string blackBoardKeyText) : base("MakeComment")
        {
            m_textMesh = textMesh;
            m_blackBoardKeyText = blackBoardKeyText;
        }

        protected override void DoStart()
        {
            if (!Blackboard.Isset(m_blackBoardKeyText))
            {
                this.Stopped(false);
            }
            string comment = (string)Blackboard.Get(m_blackBoardKeyText);
            m_textMesh.text = comment;
            this.Stopped(true);
        }
    }
}
