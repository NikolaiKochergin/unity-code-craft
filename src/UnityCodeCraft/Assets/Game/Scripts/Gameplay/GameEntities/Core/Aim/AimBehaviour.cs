using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class AimBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest<Vector3> _aimRequest;
        private Command<AimArgs> _aimCommand;

        public void Init(IGameEntity entity)
        {
            _aimRequest = entity.GetValue(GameEntityAPI.AimRequest);
            _aimCommand = entity.GetValue(GameEntityAPI.AimCommand);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if(_aimRequest.Consume(out Vector3 direction) && direction != Vector3.zero)
                _aimCommand.Invoke(new AimArgs(direction, deltaTime));
            else
                _aimCommand.Invoke(new AimArgs(Vector3.zero, deltaTime));
        }
    }
}