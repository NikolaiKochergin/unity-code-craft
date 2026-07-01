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
                if (sqrDistance > minDistance) 
                    continue;
                
                minDistance = sqrDistance;
                target = other.gameObject;
            }
            
            return target != null;
        }
        
        public static bool Attack(GameObject character, GameObject enemy, float attackDistance, float deltaTime)
        {
            Vector3 delta = enemy.transform.position - character.transform.position;
            delta.y = 0;
            
            if(delta.sqrMagnitude > attackDistance * attackDistance)
                return false;
            
            RotateTransformComponent rotateComponent = character.GetComponent<RotateTransformComponent>();
            rotateComponent.RotateTowards(enemy, deltaTime);
                    
            character.GetComponent<AttackComponent>().Attack(enemy);

            return true;
        }

        public static bool Move(GameObject character, Vector3 targetPosition, float stoppingDistance, float deltaTime)
        {
            Vector3 delta = targetPosition - character.transform.position;
            delta.y = 0;
            
            if(delta.sqrMagnitude < stoppingDistance * stoppingDistance)
                return false;

            character.GetComponent<MoveComponent>().MoveStep(delta.normalized, deltaTime);
            return true;
        }
    }
}