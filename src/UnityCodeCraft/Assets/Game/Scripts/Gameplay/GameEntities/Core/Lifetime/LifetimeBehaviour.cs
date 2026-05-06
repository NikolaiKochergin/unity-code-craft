using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public class LifetimeBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private ICooldown _cooldown;
        private IAction _destroy;
        
        public void Init(IGameEntity entity)
        {
            _cooldown = entity.GetValue(GameEntityAPI.Lifetime);
            _destroy = entity.GetValue(GameEntityAPI.DestroyAction);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            _cooldown.Tick(deltaTime);
            if (_cooldown.IsCompleted()) 
                _destroy.Invoke();
        }
    }
}