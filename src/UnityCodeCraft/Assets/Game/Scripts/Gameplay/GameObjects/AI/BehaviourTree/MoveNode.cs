using Modules.AI;
using SampleGame;
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
                !_blackboard.TryGetValue(BlackboardAPI.MovePoint, out IWayPoint movePoint) || !movePoint.IsValid)
                return BehaviourResult.Failure;

            Vector3 delta = movePoint.Position - character.transform.position;
            delta.y = 0f;
            stoppingDistance = movePoint.Size + stoppingDistance;
            float stoppingDistanceSqr = stoppingDistance * stoppingDistance;

            if (delta.sqrMagnitude <= stoppingDistanceSqr)
                return BehaviourResult.Success;
            
            character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            return BehaviourResult.Running;
        }
    }
}