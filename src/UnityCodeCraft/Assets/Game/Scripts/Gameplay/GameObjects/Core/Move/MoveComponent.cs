using System;
using Fusion;
using UnityEngine;

namespace Game
{
    public sealed class MoveComponent : NetworkBehaviour
    {
        public interface ICondition
        {
            public bool IsMet();
        }

        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _angularSpeed = 720f;
        [SerializeField] private bool _lookRotation = true;
        
        private ICondition _condition;
        
        [Networked, OnChangedRender(nameof(MoveDirectionChanged))]
        public Vector3 MoveDirection { get; private set; }
        public bool IsMoving => MoveDirection != Vector3.zero;

        public event Action OnStateChanged;

        public void SetCondition(ICondition condition) => 
            _condition = condition;

        public void Move(Vector3 direction)
        {
            MoveDirection = _condition == null || _condition.IsMet() ? direction : Vector3.zero;
            
            if(MoveDirection == Vector3.zero)
                return;

            float deltaTime = Time.fixedDeltaTime;
            UpdatePosition(MoveDirection, deltaTime);

            if (_lookRotation)
                UpdateRotation(MoveDirection, deltaTime);
        }

        public void Stop()
        {
            MoveDirection = Vector3.zero;
        }

        private void UpdatePosition(Vector3 direction, float deltaTime) => 
            transform.position += direction * deltaTime * _moveSpeed;

        private void UpdateRotation(Vector3 direction, float deltaTime)
        {
            Quaternion current = transform.rotation;
            Quaternion target = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(current, target, _angularSpeed * deltaTime);
        }

        private void MoveDirectionChanged()
        {
            OnStateChanged?.Invoke();
        }
    }
}