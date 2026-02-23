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

        public void Setup(MoveConfig config) => 
            _config = config;

        public event Action<Vector3, float> OnMoved;

        public void MoveStep(Vector2 direction) => _direction = direction;

        public void Update(float deltaTime)
        {
            if (!_direction.HasValue)
                return;

            Vector2 direction = _direction.Value;
            Vector2 newPosition = _rigidbody.position + direction * (_config.Speed * deltaTime);
            _rigidbody.MovePosition(newPosition);
            _direction = null;
            
            OnMoved?.Invoke(direction, deltaTime);
        }
    }
}