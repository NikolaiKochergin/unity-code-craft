using System;
using System.Collections.Generic;
using System.Linq;
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

        private Dictionary<Type, BehaviourNode> _nodesMap;

        protected override void OnStart()
        {
            _nodesMap = _nodes.ToDictionary(n => n.GetType());
        }

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if(!_blackboard.TryGetValue(BlackboardAPI.CommandQueue, out Queue<IAICommand> commandQueue))
                return BehaviourResult.Failure;

            if (_blackboard.TryGetValue(BlackboardAPI.CurrentCommand, out IAICommand currentCommand))
            {
                if (!_nodesMap.TryGetValue(currentCommand.NodeType, out BehaviourNode currentNode))
                    return BehaviourResult.Failure;
                
                BehaviourResult result = currentNode.Run(deltaTime);
                if (result == BehaviourResult.Running) 
                    return BehaviourResult.Running;
                    
                currentCommand.Dispose(_blackboard);
                        
                if (commandQueue.TryDequeue(out IAICommand nextCommand))
                {
                    _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, nextCommand);
                    nextCommand.Unpack(_blackboard);
                }
                else
                {
                    _blackboard.DelValue(BlackboardAPI.CurrentCommand);
                }
            }
            else if(commandQueue.TryDequeue(out IAICommand nextCommand))
            {
                _blackboard.SetReferenceValue(BlackboardAPI.CurrentCommand, nextCommand);
                nextCommand.Unpack(_blackboard);
            }
            else
            {
                if(_nodesMap.TryGetValue(typeof(GuardNode), out BehaviourNode guardNode))
                    guardNode.Run(deltaTime);
            }
            
            return BehaviourResult.Running;
        }
    }
}