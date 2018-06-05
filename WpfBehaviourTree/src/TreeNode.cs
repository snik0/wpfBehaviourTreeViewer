using System.Collections.Generic;

namespace WpfBehaviourTree.src
{
    // class describes the basic node type that is deserialised from json
    class TreeNode
    {
        public string type { get; set; }
        public List<TreeNode> children { get; set; }
    }
}
