using System;
using UnityEngine;

namespace Game
{
    public class MoveRequestComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke(Vector2 direction);
        }

        public interface ICondition
        {
            bool Evaluate();
        }

        [SerializeField]
        private Vector2 _moveDirection;

        [SerializeField]
        private bool _moveRequired;

        [SerializeField]
        private float _moveDuration = 0.1f;

        [SerializeField]
        private float _moveTime;

        private IAction _moveAction;
        private ICondition _moveCondition;
        
        public bool IsMoving => Time.time <= _moveTime;
        
        public event Action<Vector2> OnMoved;

        public void SetAction(IAction action) => _moveAction = action;
        public void SetCondition(ICondition condition) => _moveCondition = condition;

        public void Move(Vector2 direction)
        {
            _moveDirection = direction;
            _moveRequired = true;
        }

        private void FixedUpdate()
        {
            if (_moveRequired && _moveDirection != Vector2.zero &&
                (_moveCondition == null || _moveCondition.Evaluate()))
            {
                _moveAction?.Invoke(_moveDirection);
                _moveTime = Time.time + _moveDuration;
                OnMoved?.Invoke(_moveDirection);
            }
            
            _moveRequired = false;
        }
    }
}