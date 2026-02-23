using System;
using UnityEngine;

namespace Game
{
    // +
    [Serializable]
    public sealed class MoveComponent
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _speed;

        private Vector2? _direction;

        public void Setup(float speed) => 
            _speed = speed;

        public event Action<Vector3, float> OnMoved;

        public void MoveStep(Vector2 direction) => _direction = direction;

        public void Update(float deltaTime)
        {
            if (!_direction.HasValue)
                return;

            Vector2 direction = _direction.Value;
            Vector2 newPosition = _rigidbody.position + direction * (_speed * deltaTime);
            _rigidbody.MovePosition(newPosition);
            _direction = null;
            
            OnMoved?.Invoke(direction, deltaTime);
        }
    }
}