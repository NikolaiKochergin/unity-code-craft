using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public class FindClosestEnemyNode : BehaviourNode
    {
        [SerializeField]
        private Blackboard _blackboard;
        
        protected override BehaviourResult OnUpdate(float deltaTime)
        {
            Collider[] buffer = _blackboard.GetValue(BlackboardAPI.ColliderBuffer);
            int count = _blackboard.GetValue(BlackboardAPI.ColliderCount);
            GameObject character = _blackboard.GetValue(BlackboardAPI.Character);

            if (FindClosestTarget(character, buffer, count, out GameObject target))
            {
                _blackboard.SetReferenceValue(BlackboardAPI.Enemy, target);
                return BehaviourResult.Success;
            }
            
            _blackboard.DelValue(BlackboardAPI.Enemy);
            return BehaviourResult.Running;
        }
        
        private bool FindClosestTarget(
            GameObject character,
            Collider[] buffer,
            int count,
            out GameObject target)
        {
            Vector3 center = character.transform.position;
            
            target = null;
            float minDistance = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider other = buffer[i];
                if(!other || other.gameObject == character ||
                   !other.TryGetComponent(out TeamComponent teamComponent) ||
                   teamComponent.Team == _blackboard.GetValue(BlackboardAPI.Team))
                    continue;
                
                if(!other.TryGetComponent(out HealthComponent health) ||
                   !health.IsAlive)
                    continue;
                
                Vector3 delta = other.transform.position - center;
                delta.y = 0;
                
                float sqrDistance = delta.sqrMagnitude;
                if (sqrDistance < minDistance)
                {
                    minDistance = sqrDistance;
                    target = other.gameObject;
                }
            }
            
            return target != null;
        }
    }
}