using Atomic.Elements;
using Atomic.Entities;

namespace Game.Gameplay
{
    public sealed class FireBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest _request;
        private ICommand _command;
        private ICooldown _delay;

        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.FireRequest);
            _command = entity.GetValue(GameEntityAPI.FireCommand);

            entity.TryGetValue(GameEntityAPI.FireDelay, out _delay);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_request.Consume())
                if(_delay == null || _delay.IsCompleted())
                    _command.Invoke();
                else
                    _delay.Tick(deltaTime);
            else if(_delay != null && _delay.GetProgress() < 1f)
                _delay.ResetTime();
        }
    }
}