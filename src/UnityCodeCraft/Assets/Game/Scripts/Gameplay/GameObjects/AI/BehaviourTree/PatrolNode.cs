using System.Collections.Generic;
using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class PatrolNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            if (!_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<IPoint> waypoints) || 
                waypoints.Count < 2 ||
                !_blackboard.TryGetValue(BlackboardAPI.StoppingDistance, out float stoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackDistance, out float attackDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.Team, out TeamType selfTeam) ||
                !_blackboard.TryGetValue(BlackboardAPI.ColliderBuffer, out Collider[] colliderBuffer) ||
                !_blackboard.TryGetValue(BlackboardAPI.ColliderBufferSize, out int colliderBufferSize) ||
                !_blackboard.TryGetValue(BlackboardAPI.WaypointIndex, out int index) ||
                index >= waypoints.Count)
                return BehaviourResult.Failure;
            
            IPoint currentWaypoint = waypoints[index];

            if (!currentWaypoint.IsValid)
            {
                waypoints.RemoveAt(index);
                return BehaviourResult.Running;
            }
            
            if(FindUseCase.FindClosestTarget(character, selfTeam, colliderBuffer, colliderBufferSize, out GameObject target))
                _blackboard.SetReferenceValue(BlackboardAPI.Enemy, target);
            else
                _blackboard.DelValue(BlackboardAPI.Enemy);

            if (_blackboard.TryGetValue(BlackboardAPI.Enemy, out GameObject enemy) &&
                enemy.TryGetComponent(out HealthComponent health) && health.IsAlive)
            {
                if (AttackUseCase.Attack(character, enemy, attackDistance, deltaTime))
                    return BehaviourResult.Running;
                
                MoveUseCase.Move(character, enemy.transform.position, stoppingDistance, deltaTime);
                return BehaviourResult.Running;
            }
            
            if(!MoveUseCase.Move(character, currentWaypoint.Position, stoppingDistance, deltaTime))
                _blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, (index + 1) % waypoints.Count);
            
            return BehaviourResult.Running;
        }
    }
}