using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public sealed class MoveComponent : IMover
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private MoveConfig _config;
        private Vector2? _direction;
        
        public Vector2 Direction => _direction ?? Vector2.zero;

        public void Setup(MoveConfig config) => 
            _config = config;

        public void SetDirection(Vector2 direction) => 
            _direction = direction;

        public void Update(float deltaTime)
        {
            if (!_direction.HasValue)
                return;

            Vector2 direction = _direction.Value;
            Vector2 newPosition = _rigidbody.position + direction * (_config.Speed * deltaTime);
            _rigidbody.MovePosition(newPosition);
            _direction = null;
        }
    }
}