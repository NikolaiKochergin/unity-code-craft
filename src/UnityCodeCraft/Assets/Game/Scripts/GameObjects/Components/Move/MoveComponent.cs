using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(MoveTransformComponent))]
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _moveDirection;
        [SerializeField] private bool _require;
        [SerializeField] private float _moveDuration = 0.1f;
        
        private MoveTransformComponent _moveTransformComponent;
        
        private float _moveTime;
        private Func<bool> _condition;

        public bool IsMoving => Time.time <= _moveTime;
        
        public event Action<Vector2> OnMoved;

        private void Awake() => 
            _moveTransformComponent = GetComponent<MoveTransformComponent>();

        public void SetCondition(Func<bool> condition) => _condition = condition;

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
                _moveTransformComponent.Move(_moveDirection);
                _moveTime = Time.time + _moveDuration;
                OnMoved?.Invoke(_moveDirection);
            }
            
            _require = false;
        }
    }
}