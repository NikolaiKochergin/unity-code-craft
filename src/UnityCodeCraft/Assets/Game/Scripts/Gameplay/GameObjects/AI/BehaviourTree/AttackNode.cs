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
            if (_blackboard.GetValue(BlackboardAPI.CommandQueue).CurrentCommand is not AttackCommand ||
                !_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.StoppingDistance, out float stoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackDistance, out float attackDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackTarget, out IPoint targetPoint) || !targetPoint.IsValid ||
                !_blackboard.TryGetValue(BlackboardAPI.Team, out TeamType selfTeam) ||
                !_blackboard.TryGetValue(BlackboardAPI.ColliderBuffer, out Collider[] colliderBuffer) ||
                !_blackboard.TryGetValue(BlackboardAPI.ColliderBufferSize, out int colliderBufferSize))
                return BehaviourResult.Failure;
            
            if (_blackboard.TryGetValue(BlackboardAPI.Enemy, out GameObject enemy) &&
                enemy.TryGetComponent(out HealthComponent health))
            {
                if (health.IsDead)
                {
                    _blackboard.DelValue(BlackboardAPI.Enemy);
                    return BehaviourResult.Running;
                }
                
                if (NodeUseCases.Attack(character, enemy, attackDistance, deltaTime))
                    return BehaviourResult.Running;
                
                NodeUseCases.Move(character, enemy.transform.position, stoppingDistance, deltaTime);
                return BehaviourResult.Running;
            }
            
            if(NodeUseCases.FindClosestTarget(character, selfTeam, colliderBuffer, colliderBufferSize, out GameObject target))
                _blackboard.SetReferenceValue(BlackboardAPI.Enemy, target);
            
            return NodeUseCases.Move(character, targetPoint.Position, stoppingDistance, deltaTime) 
                ? BehaviourResult.Running 
                : BehaviourResult.Success;
        }
    }
}