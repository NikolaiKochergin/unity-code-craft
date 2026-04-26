using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class JumpComponent : MonoBehaviour
    {
        [SerializeField, Min(0)] private float _jumpForce = 12f;
        [SerializeField, Min(0)] private float _delay;
        [SerializeField, Min(0)] private float _cooldown;
        [SerializeField] private bool _require;

        private Rigidbody2D _rigidbody2D;
        private Func<bool> _condition;
        private float _currentTime;

        public event Action OnJump;

        private void Awake() =>
            _rigidbody2D = GetComponent<Rigidbody2D>();
        
        public void SetCondition(Func<bool> condition) => _condition = condition;

        public void Jump() => _require = true;

        private void FixedUpdate()
        {
            if (_require && IsExpired() && (_condition == null || _condition.Invoke()))
            {
                Invoke(nameof(JumpInternal), _delay);
                _currentTime = Time.time;
            }
            
            _require = false;
        }
        
        private bool IsExpired() => 
            Time.time - _currentTime > _cooldown;

        private void JumpInternal()
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            OnJump?.Invoke();
        }
    }
}