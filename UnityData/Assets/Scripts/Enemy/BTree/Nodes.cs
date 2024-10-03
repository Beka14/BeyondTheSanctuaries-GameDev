using System.Collections.Generic;

namespace Behavior
{
    public class Sequence : Node
    {
        public Sequence() { }
        public Sequence(List<Node> in_children) : base(in_children) { }

        public override NodeState Evaluate()
        {
            bool anyChildRunning = false;
            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Reset:
                        return state = NodeState.Reset;
                    case NodeState.Failure:
                        return state = NodeState.Failure;
                    case NodeState.Running:
                        anyChildRunning = true;
                        continue;
                    case NodeState.Success:
                        continue;
                    default:
                        return state = NodeState.Success;
                }
            }
            return state = anyChildRunning ? NodeState.Running : NodeState.Success;
        }
    }

    public class Selector : Node
    {
        public Selector() { }
        public Selector(List<Node> in_children) : base(in_children) { }


        public override NodeState Evaluate()
        {
            foreach (Node child in children)
            {
                switch (child.Evaluate())
                {
                    case NodeState.Reset:
                        return state = NodeState.Reset;
                    case NodeState.Running:
                        return state = NodeState.Running;
                    case NodeState.Success:
                        return state = NodeState.Success;
                    default:
                        continue;
                }
            }
            return state = NodeState.Failure;
        }
    }

    public class Invertor : Node
    {
        public Invertor() { }
        public Invertor(Node in_child) : base(new Node[1] { in_child }) { }

        public override NodeState Evaluate()
        {
            if (children.Count != 1)
            {
                return state = NodeState.Failure;
            }

            return children[0].Evaluate() switch
            {
                NodeState.Reset => state = NodeState.Reset,
                NodeState.Failure => state = NodeState.Success,
                NodeState.Success => state = NodeState.Failure,
                _ => state = NodeState.Running,
            };
        }
    }
}