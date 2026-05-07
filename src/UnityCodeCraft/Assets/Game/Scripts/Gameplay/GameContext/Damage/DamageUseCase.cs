using Atomic.Entities;

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
    }
}