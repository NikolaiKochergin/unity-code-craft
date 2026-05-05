using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class MoveBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private ICommand<MoveArgs> _moveCommand;
        private IRequest<Vector3> _moveRequest;

        public void Init(IGameEntity entity)
        {
            _moveRequest = entity.GetValue(GameEntityAPI.MoveRequest);
            _moveCommand = entity.GetValue(GameEntityAPI.MoveCommand);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            if (_moveRequest.Consume(out Vector3 direction) && direction != Vector3.zero)
            {
                entity.GetValue(GameEntityAPI.MoveSpeedMultiplier).Value = direction.magnitude;
                _moveCommand.Invoke(new MoveArgs(direction, deltaTime));
            }
            else
            {
                entity.GetValue(GameEntityAPI.MoveSpeedMultiplier).Value = 1f;
            }
        }
    }
}