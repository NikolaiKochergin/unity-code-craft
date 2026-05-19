using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public sealed class LookAtTargetNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        [SerializeField]
        [BlackboardValueKey(typeof(GameObject))]
        private string _targetKey;
        
        [SerializeField]
        [BlackboardValueKey(typeof(float))]
        private string _stoppingDistanceKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(_targetKey, out GameObject target) ||
                !_blackboard.TryGetValue(_stoppingDistanceKey, out float stoppingDistance))
                return BehaviourResult.Failure;

            Vector3 delta = target.transform.position - character.transform.position;
            delta.y = 0;

            if (delta.sqrMagnitude > stoppingDistance * stoppingDistance)
                return BehaviourResult.Failure;

            RotateTransformComponent rotateComponent = character.GetComponent<RotateTransformComponent>();
            rotateComponent.RotateTowards(target, deltaTime);
            return BehaviourResult.Running;
        }
    }
}