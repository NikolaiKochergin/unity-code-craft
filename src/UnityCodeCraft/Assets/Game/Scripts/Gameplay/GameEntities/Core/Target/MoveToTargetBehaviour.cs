using Atomic.Elements;
using Atomic.Entities;
using UnityEngine;

namespace Game.Gameplay
{
    public class MoveToTargetBehaviour : IGameEntityInit, IGameEntityFixedTick
    {
        private IVariable<IGameEntity> _target;
        private IVariable<Vector3> _position;
        private IValue<float> _attackDistance;
        private IRequest<Vector3> _moveRequest;

        public void Init(IGameEntity entity)
        {
            _target = entity.GetValue(GameEntityAPI.Target);
            _position = entity.GetValue(GameEntityAPI.Position);
            _attackDistance = entity.GetValue(GameEntityAPI.AttackDistance);
            _moveRequest = entity.GetValue(GameEntityAPI.MoveRequest);
        }

        public void FixedTick(IGameEntity entity, float deltaTime)
        {
            IGameEntity target = _target.Value;
            if(target == null)
                return;
            
            Vector3 targetPosition = target.GetValue(GameEntityAPI.Position).Value;
            Vector3 delta = targetPosition - _position.Value;
            delta.y = 0;

            if (delta.magnitude > _attackDistance.Value) 
                _moveRequest.Invoke(delta.normalized);
        }
    }
}