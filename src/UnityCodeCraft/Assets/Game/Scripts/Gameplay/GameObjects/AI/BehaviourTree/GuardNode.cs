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
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.BasePoint, out IPoint basePoint) ||
                !_blackboard.TryGetValue(BlackboardAPI.StoppingDistance, out float pointStoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.TargetStoppingDistance, out float targetStoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackDistance, out float attackDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.Team, out TeamType selfTeam) ||
                !_blackboard.TryGetValue(BlackboardAPI.ColliderBuffer, out Collider[] colliderBuffer) ||
                !_blackboard.TryGetValue(BlackboardAPI.ColliderBufferSize, out int colliderBufferSize))
                return BehaviourResult.Failure;

            if(FindUseCase.FindClosestTarget(character, selfTeam, colliderBuffer, colliderBufferSize, out GameObject target))
                _blackboard.SetReferenceValue(BlackboardAPI.Enemy, target);
            else
                _blackboard.DelValue(BlackboardAPI.Enemy);

            if (_blackboard.TryGetValue(BlackboardAPI.Enemy, out GameObject enemy) &&
                enemy.TryGetComponent(out HealthComponent health) && health.IsAlive)
            {
                if (AttackUseCase.Attack(character, enemy, attackDistance, deltaTime))
                    return BehaviourResult.Running;
                
                MoveUseCase.Move(character, enemy.transform.position, targetStoppingDistance, deltaTime);
                return BehaviourResult.Running;
            }
            
            MoveUseCase.Move(character, basePoint.Position, pointStoppingDistance, deltaTime);
            return BehaviourResult.Running;
        }
    }
}