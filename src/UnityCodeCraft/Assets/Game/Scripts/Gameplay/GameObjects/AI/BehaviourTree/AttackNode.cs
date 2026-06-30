using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class AttackNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.StoppingDistance, out float stoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackDistance, out float attackDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackTarget, out IWayPoint target) ||
                !target.IsValid)
                return BehaviourResult.Failure;
            
            Vector3 delta;
            
            if (_blackboard.TryGetValue(BlackboardAPI.Enemy, out GameObject enemy) &&
                enemy.TryGetComponent(out HealthComponent health) &&
                health.IsAlive)
            {
                delta = enemy.transform.position - character.transform.position;
                delta.y = 0;
                
                if (delta.sqrMagnitude < attackDistance * attackDistance)
                {
                    RotateTransformComponent rotateComponent = character.GetComponent<RotateTransformComponent>();
                    rotateComponent.RotateTowards(enemy, deltaTime);
                    
                    character.GetComponent<AttackComponent>().Attack(enemy);
                    return BehaviourResult.Running;
                }
            }
            else
            {
                delta = target.Position - character.transform.position;
                delta.y = 0;
            }
            
            float stoppingDistanceSqr = stoppingDistance * stoppingDistance;

            if (delta.sqrMagnitude <= stoppingDistanceSqr) 
                return BehaviourResult.Success;
            
            character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            return BehaviourResult.Running;

        }
    }
}