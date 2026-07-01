using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class HoldNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (_blackboard.GetValue(BlackboardAPI.CommandQueue).CurrentCommand is not HoldPositionCommand ||
                !_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.HoldPoint, out IWayPoint _) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackDistance, out float attackDistance))
                return BehaviourResult.Failure;

            if (!_blackboard.TryGetValue(BlackboardAPI.Enemy, out GameObject enemy) ||
                !enemy.TryGetComponent(out HealthComponent health) ||
                !health.IsAlive) 
                return BehaviourResult.Running;
            
            Vector3 delta = enemy.transform.position - character.transform.position;
            delta.y = 0;

            if (delta.sqrMagnitude > attackDistance * attackDistance) 
                return BehaviourResult.Running;
            
            RotateTransformComponent rotateComponent = character.GetComponent<RotateTransformComponent>();
            rotateComponent.RotateTowards(enemy, deltaTime);
                    
            character.GetComponent<AttackComponent>().Attack(enemy);
            return BehaviourResult.Running;
        }
    }
}