using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static partial class DamageUseCase
    {
        public static bool TakeDamage(this Collider collider, int damage, TeamType attacker) => 
            collider.TryGetComponent(out IGameEntity target) && target.TakeDamage(damage, attacker);

        public static bool TakeDamage(this IGameEntity target, int damage, TeamType attacker)
        {
            if(!target.HasTag(GameEntityAPI.DamageableTag))
                return false;

            TeamType victim = target.GetValue(GameEntityAPI.Team).Value;
            if(attacker == victim)
                return false;

            target.GetValue(GameEntityAPI.TakeDamageCommand).Invoke(damage);
            return true;
        }
    }
}