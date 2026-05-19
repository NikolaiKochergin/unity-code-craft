using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class MoveToPositionNode : BehaviourNode
    {
        private const float ArriveDistanceSqr = 0.01f;
        
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        [BlackboardValueKey(typeof(Vector3))]
        private string _positionKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if(!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
               !_blackboard.TryGetValue(_positionKey, out Vector3 position))
                return BehaviourResult.Failure;
            
            Vector3 delta = position - character.transform.position;
            delta.y = 0;

            if (delta.sqrMagnitude < ArriveDistanceSqr)
                return BehaviourResult.Success;
            
            character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            return BehaviourResult.Running;
        }
    }
}