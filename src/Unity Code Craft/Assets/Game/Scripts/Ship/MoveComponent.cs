using UnityEngine;

namespace Game
{
    public sealed class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        private MoveConfig _config;
        private Vector2? _direction;
        
        public Vector2 Direction => _direction ?? Vector2.zero;

        public void Setup(MoveConfig config) => 
            _config = config;

        public void SetDirection(Vector2 direction) => 
            _direction = direction;

        public void Move()
        {
            if (!_direction.HasValue)
                return;

            Vector2 direction = _direction.Value;
            Vector2 newPosition = _rigidbody.position + direction * (_config.Speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPosition);
            _direction = null;
        }
    }
}