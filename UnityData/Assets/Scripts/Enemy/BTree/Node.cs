using System.Collections.Generic;

namespace Behavior
{
    public enum NodeState
    {
        Running,
        Success,
        Failure,
        Reset
    }

    public class Node
    {
        protected NodeState state;
        protected Node parent;
        protected List<Node> children = new();
        protected BlackBoard blackboard = null;

        public NodeState State
        {
            get { return state; }
        }
        public Node Parent
        {
            get { return parent; }
        }
        public Node Root
        {
            get
            {
                Node current = this;
                while (current.parent != null)
                    current = current.parent;
                return current;
            }
        }

        public List<Node> Children
        {
            get { return children; }
        }


        public Node()
        {
        }
        public Node(IEnumerable<Node> in_children)
        {
            foreach (Node child in in_children)
                AttachChild(child);
        }
        public virtual NodeState Evaluate() => NodeState.Failure;


        public void SetBlackBoard(BlackBoard bb)
        {
            blackboard = bb;
            foreach (Node child in children)
                child.SetBlackBoard(bb);
        }
        private void AttachChild(Node child)
        {
            children.Add(child);
            child.parent = this;
        }
    }

}