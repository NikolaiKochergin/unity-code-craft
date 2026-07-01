using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class GuardNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.GetValue(BlackboardAPI.CommandQueue).IsEmpty ||
                !_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.BasePoint, out IWayPoint basePoint) ||
                !_blackboard.TryGetValue(BlackboardAPI.StoppingDistance, out float pointStoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.TargetStoppingDistance, out float targetStoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackDistance, out float attackDistance))
                return BehaviourResult.Failure;

            Vector3 delta;
            float stoppingDistanceSqr;
            
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
                
                stoppingDistanceSqr = targetStoppingDistance * targetStoppingDistance;
            }
            else
            {
                delta = basePoint.Position - character.transform.position;
                delta.y = 0;
                
                stoppingDistanceSqr = pointStoppingDistance * pointStoppingDistance;
            }
            
            if (delta.sqrMagnitude > stoppingDistanceSqr) 
                character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            
            return BehaviourResult.Running;
        }
    }
}