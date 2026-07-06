using Modules.AI;
using UnityEngine;

namespace Game
{
    public class MoveNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.StoppingDistance, out float stoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.MovePoint, out IPoint movePoint) || !movePoint.IsValid)
                return BehaviourResult.Failure;

            return MoveUseCase.Move(character, movePoint.Position, stoppingDistance, deltaTime) 
                ? BehaviourResult.Running 
                : BehaviourResult.Success;
        }
    }
}