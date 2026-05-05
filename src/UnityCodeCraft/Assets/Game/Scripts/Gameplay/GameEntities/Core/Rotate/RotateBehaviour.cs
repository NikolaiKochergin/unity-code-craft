using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class RotateBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IRequest<Vector3> _request;
        private ICommand<RotateArgs> _command;

        public void Init(IGameEntity entity)
        {
            _request = entity.GetValue(GameEntityAPI.RotateRequest);
            _command = entity.GetValue(GameEntityAPI.RotateCommand);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if(_request.Consume(out Vector3 direction) && direction != Vector3.zero)
                _command.Invoke(new RotateArgs(direction, deltaTime));
        }
    }
}