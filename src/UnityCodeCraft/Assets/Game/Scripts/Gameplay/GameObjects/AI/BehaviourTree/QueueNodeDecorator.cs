using Modules.AI;
using UnityEngine;

namespace Game
{
    public sealed class QueueNodeDecorator : BehaviourNode, IBehaviourNodeDecorator
    {
        public BehaviourNode Child => _origin;
        
        [SerializeField]
        private Blackboard _blackboard;
        
        [Space]
        [SerializeField]
        private BehaviourNode _origin;
        
        protected override void OnStart() => 
            _blackboard.GetValue(BlackboardAPI.CommandQueue).TryStartNext();

        protected override BehaviourResult OnUpdate(float deltaTime) => 
            _origin.Run(deltaTime);
    }
}