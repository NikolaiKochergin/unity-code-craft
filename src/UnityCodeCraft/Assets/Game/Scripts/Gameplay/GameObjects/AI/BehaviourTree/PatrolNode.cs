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
            if (_blackboard.GetValue(BlackboardAPI.CommandQueue).CurrentCommand is not PatrolCommand ||
                !_blackboard.TryGetValue(BlackboardAPI.Character, out GameObject character) ||
                !_blackboard.TryGetValue(BlackboardAPI.Waypoints, out List<IWayPoint> waypoints) || 
                waypoints.Count < 2 ||
                !_blackboard.TryGetValue(BlackboardAPI.StoppingDistance, out float stoppingDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.AttackDistance, out float attackDistance) ||
                !_blackboard.TryGetValue(BlackboardAPI.WaypointIndex, out int index) ||
                index >= waypoints.Count)
                return BehaviourResult.Failure;
            
            IWayPoint currentWaypoint = waypoints[index];

            if (!currentWaypoint.IsValid)
            {
                waypoints.RemoveAt(index);
                return BehaviourResult.Running;
            }
            
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
                delta = currentWaypoint.Position - character.transform.position;
                delta.y = 0f;
                stoppingDistance = currentWaypoint.Size + stoppingDistance;
                float stoppingDistanceSqr = stoppingDistance * stoppingDistance;

                if (delta.sqrMagnitude <= stoppingDistanceSqr)
                {
                    _blackboard.SetPrimitiveValue(BlackboardAPI.WaypointIndex, (index + 1) % waypoints.Count);
                    return BehaviourResult.Running;
                }
            }

            character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            return BehaviourResult.Running;
        }
    }
}