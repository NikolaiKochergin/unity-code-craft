using SampleGame;
using UnityEngine;

namespace Game
{
    public static class NodeUseCases
    {
        public static bool FindClosestTarget(
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