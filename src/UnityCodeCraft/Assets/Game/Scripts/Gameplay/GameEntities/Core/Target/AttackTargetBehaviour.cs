using Atomic.Entities;

namespace Game.Gameplay
{
    public class AttackTargetBehaviour : IGameEntityFixedTick
    {
        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (entity.IsInAttackDistance() && entity.IsTargetAlive())
                entity.GetValue(GameEntityAPI.FireRequest).Invoke();
        }
    }
}