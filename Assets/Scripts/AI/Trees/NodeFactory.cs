using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;

namespace AI.Trees.NodeFactory
{
    public class NodeFactory
    {
        public static Node CreateMoveSubtree()
        {
            // shared movement behavior, you can create any trees of any depth
            return new Cooldown(3.0f,
                new Action(() => Debug.Log(" moving!"))
            );
        }
    }
}
