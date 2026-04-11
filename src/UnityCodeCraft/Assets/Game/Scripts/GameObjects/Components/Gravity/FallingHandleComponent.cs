using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(ExtraGravityComponent), typeof(Rigidbody2D))]
    public class FallingHandleComponent : MonoBehaviour
    {
        public interface IAction
        {
            void Invoke(bool isFalling);
        }
        
        [SerializeField, Min(0)] private float _velocityCheckDelta = 0.01f;
        
        private Rigidbody2D _rigidbody;
        private IAction _fallingAction;
        private bool _isFalling;

        public bool IsFalling
        {
            get => _isFalling;
            set
            {
                if(value == _isFalling)
                    return;
                
                _isFalling = value;
                _fallingAction?.Invoke(value);
                OnFalling?.Invoke(value);
            }
        }
        
        public event Action<bool> OnFalling;

        private void Awake() => 
            _rigidbody = GetComponent<Rigidbody2D>();

        private void FixedUpdate() => 
            IsFalling = _rigidbody && _rigidbody.linearVelocityY < -_velocityCheckDelta;
        
        public void SetAction(IAction action) => _fallingAction = action;
    }
}
