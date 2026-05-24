using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public sealed class AttackTargetNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;

        [SerializeField]
        [BlackboardValueKey(typeof(float))]
        private string _distanceKey;

        [SerializeField]
        [BlackboardValueKey(typeof(GameObject))]
        private string _targetKey;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(_targetKey, out GameObject target) ||
                !_blackboard.TryGetValue(_distanceKey, out float attackDistance))
                return BehaviourResult.Failure;

            if (!target.TryGetComponent(out HealthComponent health) || health.IsDead)
                return BehaviourResult.Success;

            Vector3 delta = target.transform.position - character.transform.position;
            delta.y = 0;

            if (delta.sqrMagnitude > attackDistance * attackDistance)
                return BehaviourResult.Failure;

            AttackComponent component = character.GetComponent<AttackComponent>();
            
            component.Attack(target);
            return BehaviourResult.Running;
        }
    }
}