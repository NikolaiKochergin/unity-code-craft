using Modules.AI;
using UnityEngine;

namespace Game
{
    public class CompleteMoveToTargetNode : BehaviourNode
    {
        [SerializeField] 
        private Blackboard _blackboard;

        [SerializeField] 
        [BlackboardValueKey(typeof(GameObject))]
        private string _targetKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            _blackboard.DelValue(_targetKey);
            return BehaviourResult.Success;
        }
    }
}