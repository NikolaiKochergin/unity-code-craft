using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class FollowNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.TargetStoppingDistance, out float stoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.FollowTarget, out GameObject target) ||
                !target)
                return BehaviourResult.Failure;
            
            Vector3 delta = target.transform.position - character.transform.position;
            delta.y = 0f;
            float stoppingDistanceSqr = stoppingDistance * stoppingDistance;

            if (delta.sqrMagnitude > stoppingDistanceSqr)
                character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            
            return BehaviourResult.Running;
        }
    }
}