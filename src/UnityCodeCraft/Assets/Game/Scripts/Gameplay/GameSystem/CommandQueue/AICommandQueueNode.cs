using System.Collections.Generic;
using Modules.AI;
using UnityEngine;

namespace Game
{
    public sealed class AICommandQueueNode : BehaviourNode, IBehaviourNodeComposite
    {
        public IEnumerable<BehaviourNode> Nodes => _nodes;
        
        [SerializeField]
        private Blackboard _blackboard;
        
        [Space]
        [SerializeField]
        private BehaviourNode[] _nodes;
        
        
        protected override void OnStart()
        {
            
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            
            
            
            
            return BehaviourResult.Running;
        }
    }
}