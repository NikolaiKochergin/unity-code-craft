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

        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(_targetKey, out GameObject target))
                return BehaviourResult.Failure;

            RotateTransformComponent rotateComponent = character.GetComponent<RotateTransformComponent>();
            rotateComponent.RotateTowards(target, deltaTime);
            return BehaviourResult.Running;
        }
    }
}