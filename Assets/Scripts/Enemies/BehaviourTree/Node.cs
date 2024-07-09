using System.Collections.Generic;
using UnityEngine;

namespace Enemies.BehaviourTree
{
    public class Node {
        public enum Status { Success, Failure, Running }
        
        public readonly string name;
        public readonly int priority;
        public readonly List<Node> children = new();
        protected int currentChild;
        
        public Node(string name = "Node", int priority = 0) {
            this.name = name;
            this.priority = priority;
        }
        
        public void AddChild(Node child) => children.Add(child);
        public virtual Status Process() => children[currentChild].Process();
        public virtual void Reset() {
            currentChild = 0;
            foreach (var child in children) {
                child.Reset();
            }
        }
    }

    public class Leaf : Node
    {
        public readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy, int priority = 0) : base(name, priority) {
            this.strategy = strategy;
        }

        public override Status Process() => strategy.Process();
        public override void Reset() => strategy.Reset();
    }

    public class Sequence : Node
    {
        public Sequence (string name, int priority = 0) : base(name, priority) { }

        public override Status Process()
        {
            if (currentChild < children.Count)
            {
                switch (children[currentChild].Process())
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    default:
                        currentChild++;
                        return (currentChild == children.Count) ? Status.Success : Status.Running;
                }
            }
            
            Reset();
            return Status.Success;
        }
    }

    public class Selector : Node {
        public Selector(string name, int priority = 0) : base(name, priority) { }

        public override Status Process() {
            if (currentChild < children.Count) {
                switch (children[currentChild].Process()) {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        currentChild++;
                        return Status.Running;
                }
            }
            
            Reset();
            return Status.Failure;
        }
    }
    
    public class BehaviourTree : Node
    {
        private readonly IPolicy _policy;

        public BehaviourTree(string name, IPolicy policy = null) : base(name) {
            _policy = policy ?? Policies.RunForever;
        }

        public override Status Process() {
            Status status = children[currentChild].Process();
            if (_policy.ShouldReturn(status)) {
                return status;
            }
            
            currentChild = (currentChild + 1) % children.Count;
            return Status.Running;
        }
    }
    
    public interface IPolicy {
        bool ShouldReturn(Node.Status status);
    }
    
    public static class Policies {
        public static readonly IPolicy RunForever = new RunForeverPolicy();
        public static readonly IPolicy RunUntilSuccess = new RunUntilSuccessPolicy();
        public static readonly IPolicy RunUntilFailure = new RunUntilFailurePolicy();
        
        class RunForeverPolicy : IPolicy {
            public bool ShouldReturn(Node.Status status) => false;
        }
        
        class RunUntilSuccessPolicy : IPolicy {
            public bool ShouldReturn(Node.Status status) => status == Node.Status.Success;
        }
        
        class RunUntilFailurePolicy : IPolicy {
            public bool ShouldReturn(Node.Status status) => status == Node.Status.Failure;
        }
    }
}