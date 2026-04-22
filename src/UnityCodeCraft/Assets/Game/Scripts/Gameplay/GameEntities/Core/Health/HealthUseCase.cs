using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static class HealthUseCase
    {
        public static bool IsHealthExists(this IGameEntity entity) =>
            entity.GetValue(GameEntityAPI.CurrentHealth).Value > 0;

        public static bool IsDead(this IGameEntity entity) =>
            entity.GetValue(GameEntityAPI.CurrentHealth).Value <= 0;

        public static bool ReduceHealth(this IGameEntity entity, int damage)
        {
            if(!entity.IsHealthExists())
                return false;
            
            IVariable<int> health = entity.GetValue(GameEntityAPI.CurrentHealth);
            int newHealth = Mathf.Max(0, health.Value - damage);
            health.Value = newHealth;
            return true;
        }
    }
}