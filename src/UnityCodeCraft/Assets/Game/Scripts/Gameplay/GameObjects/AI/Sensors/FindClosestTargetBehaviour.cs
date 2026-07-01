using Modules.AI;
using SampleGame;
using UnityEngine;

namespace Game
{
    public sealed class FindClosestTargetBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Blackboard _blackboard;

        private void FixedUpdate()
        {
            Collider[] buffer = _blackboard.GetValue(BlackboardAPI.ColliderBuffer);
            int count = _blackboard.GetValue(BlackboardAPI.ColliderBufferSize);
            GameObject character = _blackboard.GetValue(BlackboardAPI.Character);
            TeamType selfTeam = _blackboard.GetValue(BlackboardAPI.Team);

            if (FindClosestTarget(character, selfTeam, buffer, count, out GameObject target))
                _blackboard.SetReferenceValue(BlackboardAPI.Enemy, target);
            else
                _blackboard.DelValue(BlackboardAPI.Enemy);
        }

        private static bool FindClosestTarget(
            GameObject character,
            TeamType selfTeam,
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
                   teamComponent.Team == selfTeam)
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