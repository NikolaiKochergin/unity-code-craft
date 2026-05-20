using Modules.AI;
using UnityEngine;

namespace Game
{
    public sealed class WaitingForTarget : BehaviourNode
    {
        [SerializeField] 
        private Blackboard _blackboard;

        [SerializeField] 
        [BlackboardValueKey(typeof(GameObject))]
        private string _targetKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime) => 
            _blackboard.TryGetValue(_targetKey, out _) 
                ? BehaviourResult.Success 
                : BehaviourResult.Running;
    }
}