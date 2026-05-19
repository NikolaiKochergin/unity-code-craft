using Modules.AI;
using UnityEngine;

namespace Game
{
    public class CompleteMoveToPointNode : BehaviourNode
    {
        [SerializeField] 
        private Blackboard _blackboard;

        [SerializeField] 
        [BlackboardValueKey(typeof(Vector3))]
        private string _targetKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            _blackboard.DelValue(_targetKey);
            return BehaviourResult.Success;
        }
    }
}