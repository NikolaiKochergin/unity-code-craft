using Modules.AI;
using UnityEngine;

namespace Game
{
    public class FollowNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.GetValue(BlackboardAPI.CommandQueue).CurrentCommand is not FollowCommand ||
                !_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.TargetStoppingDistance, out float stoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.FollowTarget, out GameObject target) ||
                !target)
                return BehaviourResult.Failure;
            
            NodeUseCases.Move(character, target.transform.position, stoppingDistance, deltaTime);
            return BehaviourResult.Running;
        }
    }
}