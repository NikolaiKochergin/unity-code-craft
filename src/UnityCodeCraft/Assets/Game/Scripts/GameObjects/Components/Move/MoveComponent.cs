using System;
using UnityEngine;

namespace Game
{
    public sealed class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _moveDirection;
        [SerializeField] private bool _require;
        [SerializeField] private float _moveDuration = 0.1f;
        
        private float _moveTime;
        private Func<bool> _condition;
        private Action<Vector2> _action;

        public bool IsMoving => Time.time <= _moveTime;
        
        public event Action<Vector2> OnMoved;

        public void SetCondition(Func<bool> condition) => _condition = condition;
        public void SetAction(Action<Vector2> action) => _action = action;

        public void Move(Vector2 direction)
        {
            _moveDirection = direction;
            _require = true;
        }

        private void FixedUpdate()
        {
            if (_require && _moveDirection != Vector2.zero &&
                (_condition == null || _condition.Invoke()))
            {
                _action?.Invoke(_moveDirection);
                _moveTime = Time.time + _moveDuration;
                OnMoved?.Invoke(_moveDirection);
            }
            
            _require = false;
        }
    }
}