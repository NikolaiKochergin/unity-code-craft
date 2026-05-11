using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class FireUseCase
    {
        public static bool IsInAttackDistance(this IGameEntity entity)
        {
            if(!entity.TryGetValue(GameEntityAPI.Target, out IVariable<IGameEntity> target) || target.Value == null)
                return false;

            Vector3 targetPosition = target.Value.GetValue(GameEntityAPI.Position).Value;
            Vector3 selfPosition = entity.GetValue(GameEntityAPI.Position).Value;
            float attackDistance = entity.GetValue(GameEntityAPI.AttackDistance).Value;
            
            return attackDistance >= Vector3.Distance(selfPosition, targetPosition);
        }
    }
}