using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public static partial class DamageUseCase
    {
        public static bool TakeDamage(this IGameContext gameContext, IGameEntity target, int damage, TeamType instigator)
        {
            if(!target.TakeDamage(damage, instigator))
                return false;

            if (target.IsDead())
                gameContext.ProcessKill(new KillArgs(
                    killer: instigator, 
                    victim: target.GetValue(GameEntityAPI.Team).Value)
                );
            
            return true;
        }
        
        public static void DealDamageOverlap(this IGameEntity entity, Collider[] results, float radius, int damage)
        {
            Vector3 position = entity.GetValue(GameEntityAPI.Position).Value;
            TeamType instigator = entity.GetValue(GameEntityAPI.Team).Value;
            
            int size = Physics.OverlapSphereNonAlloc(position, radius, results);
            for (int i = 0; i < size; i++)
                if (results[i].TryGetComponent(out IGameEntity target) && target.HasTag(GameEntityAPI.CharacterTag))
                    if(GameContext.Instance.TakeDamage(target, damage, instigator))
                        break;
        }
    }
}