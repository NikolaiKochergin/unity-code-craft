using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class MoveToTargetNode : BehaviourNode
    {
        [SerializeField] 
        private Blackboard _blackboard;

        [SerializeField] 
        [BlackboardValueKey(typeof(GameObject))]
        private string _targetKey;

        [SerializeField] 
        [BlackboardValueKey(typeof(float))]
        private string _stoppingDistance;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(_targetKey, out GameObject target) || !target ||
                !_blackboard.TryGetValue(_stoppingDistance, out float stoppingDistance))
                return BehaviourResult.Failure;
            
            Vector3 delta = target.transform.position - character.transform.position;
            delta.y = 0;

            if (delta.sqrMagnitude <= stoppingDistance * stoppingDistance)
                return BehaviourResult.Success;
            
            character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            return BehaviourResult.Running;
        }
    }
}